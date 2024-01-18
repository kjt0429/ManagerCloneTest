using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace hive.manager {
    //ignore 목록을 구분하기 위한 tree
    class HIVEIgnorePathTree { 
        private string data = "";
        List<HIVEIgnorePathTree> child = new List<HIVEIgnorePathTree>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths">ignore로 등록할 경로</param>
        /// <param name="cur">재귀로 처리중인 paths의 순번</param>
        private void insert(string[] paths, int cur) {
            bool bFind = false;
            bool isLastPath = paths.Length-1 == cur;
            string path = paths[cur];
            //TODO: trie가 더 효율적?
            for (int i=0;i<child.Count;++i) {
                if(child[i].data == path) {
                    if (isLastPath) {
                        //마지막인데 자식이 존재할경우 자식을을 모두 제거해준다.
                        //가능하면 발생하지 않도록 정렬후 넣어줄것을 추천.
                        if(child[i].child.Count > 0){
                            child[i].child.Clear();
                        }
                    } else {
                        // 마지막 패스일경우 더이상 추가하지 않는다.
                        if (child[i].child.Count > 0) {
                            child[i].insert(paths, cur+1);
                        }
                    }
                    bFind = true;
                    break;
                }
            }

            if (bFind == false) {
                //새로운 자식 추가.
                var c = new HIVEIgnorePathTree();
                c.data = path;
                child.Add(c);
                if (!isLastPath) {
                    //마지막이 아니라면 하위노드 인서트할수있도록
                    c.insert(paths, cur+1);
                }
            }
        }

        private string ChangePathWindowsToUnix(string path)
        {
            return path.Replace('\\', '/'); //carriage return change, windows to unix
        }
        public void insert(string path) {
            path = ChangePathWindowsToUnix(path);
            var paths = path.Split('/');
            insert(paths, 0);
        }

        private bool isIgnore(string[] paths,int cur) {
            var path = paths[cur];
            var isLastPath = paths.Length-1 == cur;
            for (int i=0;i<child.Count;++i) {
                if (child[i].data == path) {
                    if (child[i].child.Count == 0) {
                        //하위패스가 존재하지 않으므로 무시가능 목록임.
                        return true;
                    } else {
                        if (isLastPath) {
                            //문자열 마지막에 마지막에 하위노드 존재시 ignore 대상 아님.
                            return false;
                        } else {
                            //다음패스로 진행
                            return child[i].isIgnore(paths, cur+1);
                        }
                    }
                    break;
                }
            } 
            return false;
        }

        public bool isIgnore(string path) {
            path = ChangePathWindowsToUnix(path);
            var paths = path.Split('/');
            return isIgnore(paths, 0);
        }

        [TestFixture]
        class IgnorePathTreeTest {
            [Test] 
            public void ignorePathTreeTest() {
                string[] ignoreList = {
                    "abcd/abcd/abcd/bbb/dddd",
                    "abcd/abcd/abcd/bbb",
                    "abcd/abcd/abcd/bbb/cccc",
                    "abcd/abcd/abcd/bbb/eeee",
                    "abcde/adddd/1234"
                };

                string[] ignoreFilesList = {
                    "abcd/abcd/abcd/bbb/text111",
                    "abcd/abcd/abcd/bbb/cccc/tttt22",
                    "abcd/abcd/abcd/bbb/dddd/dfdf",
                    "abcd/abcd/abcd/bbb/eeee/dddd.txt",
                    "abcde/adddd/1234/ee.txt"
                };

                string[] nonIgnoreFilesList = {
                    "abcd/abcd/abcd/ddd",
                    "abcd/abcd/abcd",
                    "abcd/ede",
                    "abcd/abcd/abcd/bbb.cs",
                    "abcde/adddd/1234.txt"
                };

                var pathTree = new HIVEIgnorePathTree();
                for (int i=0;i<ignoreList.Length;++i) {
                    pathTree.insert(ignoreList[i]);
                }

                for (int i=0;i<ignoreFilesList.Length;++i) {
                    Assert.AreEqual(true, pathTree.isIgnore(ignoreFilesList[i]));
                }
                
                for (int i=0;i<nonIgnoreFilesList.Length;++i) {
                    Assert.AreEqual(false, pathTree.isIgnore(nonIgnoreFilesList[i]));
                }
            }
        }
    }
    /// <summary>
    /// 패치파일을 로드하고 파싱한다.
    /// parse 함수를 사용하여 파싱하며
    /// isIgnore 를 통해 ignore 파일인지 확인한다.
    /// </summary>
    class HIVEIgnorePatchFile {

        bool isCaseSensetive;
        HIVEIgnorePathTree pathTree = new HIVEIgnorePathTree();
        string fileName;

        public HIVEIgnorePatchFile(bool isCaseSensetive) {
            this.isCaseSensetive = isCaseSensetive;
        }

        public HIVEIgnorePatchFile(string ignoreFile,bool isCaseSensetive) {
            fileName = ignoreFile;
            this.isCaseSensetive = isCaseSensetive;
        }

        private string getRemoveCommentAndTrim(string message) {
            message.Trim();
            int index = message.IndexOf('#');
            if (index != -1) {
                return message.Substring(0, index).Trim();
            }
            return message;
        }

        private string pathStandard(string path) {
            if (isCaseSensetive) {
                
                return Path.GetFullPath(path);
            } else {
                return Path.GetFullPath(path.ToLower());
            }
        }

        public void setIgnoreFileName(string file) {
            fileName = file;
        }

        public void insertIgnorePath(string ignore) {
            pathTree.insert(pathStandard(ignore));
        }

        public void parse() {
            if (File.Exists(fileName)) {
                List<string> ignoreList = new List<string>();
                using (StreamReader sr = File.OpenText(fileName)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        // #부터는 주석으로 처리함
                        var commentRevmoeLine = getRemoveCommentAndTrim(line);
                        if(!string.IsNullOrEmpty(commentRevmoeLine)) {
                            var path = pathStandard(commentRevmoeLine);
                            ignoreList.Add(path);
                            // .meta파일도 같이 무시목록으로 등록해줘야한다.
                            var meta = path+".meta";
                            ignoreList.Add(meta);
                        }
                    }
                }
                ignoreList.Sort();
                //무시목록 패스 작성.
                for (int i=0;i<ignoreList.Count;++i) {
                    Debug.Log("Register HIVE SDK Ignored : " + ignoreList[i]);
                    pathTree.insert(ignoreList[i]);
                }
            }
        }

        public bool isIgnore(string path) {
            //MAKE PATH TO ABSOULTE PATH
            return pathTree.isIgnore(pathStandard(path));
        }

        [TestFixture]
        class IgnorePatchFileTest {
            [Test] 
            public void ignorePathTreeTest() {

                TestContext.WriteLine("ready");

                string[] comment = {
                    "./advb/adadf/adfadf/adf.txt# comment comment",
                    "./advb/adadf/adfadf/adf.txt # double comment # double comment",
                    "# comment start",
                    "##### ## ######",
                    "abcde/adddd/1234"

                };

                string[] expectedRemoveComment = {
                    "./advb/adadf/adfadf/adf.txt",
                    "./advb/adadf/adfadf/adf.txt",
                    "",
                    "",
                    "abcde/adddd/1234"
                };

                

                var patchFile = new HIVEIgnorePatchFile("", false);
                for (int i=0;i<comment.Length;++i) {
                    var c = patchFile.getRemoveCommentAndTrim(comment[i]);
                    TestContext.WriteLine("getRemoveComment : " + comment[i] +" -> "+ c + " -> " + expectedRemoveComment[i]);
                    Assert.AreEqual(expectedRemoveComment[i], c);
                }
            }
        }
    }
}