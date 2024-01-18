using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace hive.manager {
    // https://semver.org/ 의 규칙에 따라 구현된 클래스
    // 단, 하이브에서는 Sementic Version의 맨 앞은 Brand 버전임.
    public class HIVESemanticVersion  : System.IComparable {
        public int Brand { get; private set; }

        public int Major { get; private set; }

        public int Minor { get; private set; }

        public int Patch { get; private set; }

        public PreReleaseField PreRelease { get; private set; }

        public string BuildMetadata = null;

        public HIVESemanticVersion(string version) {
            try {
                init(version);
            } catch {
                throw;
            }
        }

        private void init(string version) {
            if(string.IsNullOrEmpty(version)) throw new System.ArgumentException("version");

            string[] metaSplit = version.Split('+');
            if(metaSplit.Length >= 2) {
                BuildMetadata = metaSplit[1];
            }
            
            string[] preReleaseSplit = metaSplit[0].Split('-');
            if(preReleaseSplit.Length >= 2) {
                if(!string.IsNullOrEmpty(preReleaseSplit[1]))
                    PreRelease = new PreReleaseField(preReleaseSplit[1]);
            }

            string[] v = preReleaseSplit[0].Split('.');
            if(v.Length != 3 && v.Length != 4) {
                throw new System.ArgumentException("version parse error. version length : "+v.Length.ToString());
            }
            int brand,major,minor,patch;
            if(!int.TryParse(v[0],out brand)) throw new System.ArgumentException("version");

            if(!int.TryParse(v[1],out major)) throw new System.ArgumentException("version");

            if(!int.TryParse(v[2],out minor)) throw new System.ArgumentException("version");

            if( v.Length == 4 ) {
                if(!int.TryParse(v[3],out patch)) throw new System.ArgumentException("version");
            } else {
                patch = 0;
            }
            
            Brand = brand;
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public static bool operator ==(HIVESemanticVersion v1, HIVESemanticVersion v2) {
            if (ReferenceEquals(v1,v2)) return true;
            if ((object)v1 == null) return false;
            return v1.Equals(v2);
        }

        public static bool operator !=(HIVESemanticVersion v1, HIVESemanticVersion v2) {
            return !(v1 == v2);
        }

        public static bool operator <(HIVESemanticVersion v1, HIVESemanticVersion v2) {
            return v1.CompareTo(v2) < 0;
        }
        public static bool operator >(HIVESemanticVersion v1, HIVESemanticVersion v2) {
            return v1.CompareTo(v2) > 0;
        }

        public static bool operator <=(HIVESemanticVersion v1, HIVESemanticVersion v2) {
            return v1.CompareTo(v2) <= 0;
        }
        public static bool operator >=(HIVESemanticVersion v1, HIVESemanticVersion v2) {
            return v1.CompareTo(v2) >= 0;
        }

        public int CompareTo(object obj) {

            HIVESemanticVersion other = (HIVESemanticVersion)obj;

            if(other == null) throw new System.ArgumentException("obj");

            if (this.Brand != other.Brand) {
                return this.Brand.CompareTo(other.Brand);
            } else if (this.Major != other.Major) {
                return this.Major.CompareTo(other.Major);
            } else if (this.Minor != other.Minor) {
                return this.Minor.CompareTo(other.Minor);
            } else if (this.Patch != other.Patch) {
                return this.Patch.CompareTo(other.Patch);
            } else if (this.PreRelease != other.PreRelease) {
                if (PreRelease != null && other.PreRelease != null){
                    return this.PreRelease.CompareTo(other.PreRelease);
                } else if (this.PreRelease != null) {
                    return -1;
                } else {
                    return 1;
                }
            }
            return 0;
        }

        public override bool Equals(object obj) {

            if (obj == null) return false;

            HIVESemanticVersion v = obj as HIVESemanticVersion;
            
            if (v == null) return false;

            if (ReferenceEquals(this,obj)) return true;

            return IgnoreMetadataEquals(obj) 
                    && (this.BuildMetadata == v.BuildMetadata);
        }

        public bool IgnoreMetadataEquals(object obj) {
            if(obj == null) return false;

            HIVESemanticVersion v = obj as HIVESemanticVersion;
            
            if(v == null) return false;

            if(ReferenceEquals(this,obj)) return true;

            return (this.Brand == v.Brand
                    && this.Major == v.Major
                    && this.Minor == v.Minor
                    && this.Patch == v.Patch
                    && this.PreRelease == v.PreRelease);
        }

        public override int GetHashCode() {
            return this.Brand.GetHashCode() ^ this.Major.GetHashCode() ^ this.Minor.GetHashCode()
                ^ this.Patch.GetHashCode() ^ this.PreRelease.GetHashCode();
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append(Brand);
            builder.Append(".");
            builder.Append(Major);
            builder.Append(".");
            builder.Append(Minor);
            if (Patch != 0) {
                // Patch 가 0일경우 버전표기에서 생략. 
                // 4.11.2(.0) < Patch버전이 0이라 생략됨
                // 4.11.2.1 < Patch가 0이 아니라서 생략불가.
                builder.Append(".");
                builder.Append(Patch);
            }

            if(PreRelease != null) {
                builder.Append("-");
                builder.Append(PreRelease.ToString());
            }
            
            if(!string.IsNullOrEmpty(BuildMetadata)) {
                builder.Append("+");
                builder.Append(BuildMetadata);
            }

            return builder.ToString();
        }

        public static implicit operator string(HIVESemanticVersion version) {
            if(version != null) return version.ToString();
            return null;
        }

        public static implicit operator HIVESemanticVersion(string version) {
            return new HIVESemanticVersion(version);
        }
    }

    public static class SemanticVersionFromString {
        public static HIVESemanticVersion ToSemanticVersion(this string version) {
            return version;
        }
    }

    public class PreReleaseField : System.IComparable {
        string originalValue;
        string[] seperatedValue;

        public static bool operator ==(PreReleaseField v1, PreReleaseField v2) {
            if(ReferenceEquals(v1,v2)) return true;

            if((object)v1 == null) return false; 

            return v1.Equals(v2);
        }

        public static bool operator !=(PreReleaseField v1, PreReleaseField v2) {
            return !(v1 == v2);
        }

        public static bool operator <(PreReleaseField v1, PreReleaseField v2) {
            return v1.CompareTo(v2) < 0;
        }

        public static bool operator >(PreReleaseField v1, PreReleaseField v2) {
            return v1.CompareTo(v2) > 0;
        }

        public PreReleaseField(string preRelease) {
            if(preRelease == null) throw new System.ArgumentException("preRelease");
            originalValue = preRelease;
            seperatedValue = preRelease.Split('.');
        }

        public int CompareTo(object obj) {
            PreReleaseField other = (PreReleaseField)obj;
            if(other == null) throw new System.ArgumentException("obj");

            int min = System.Math.Min(this.seperatedValue.Length, other.seperatedValue.Length);

            for(int i = 0;i<min;++i) {
                if(this.seperatedValue[i].Equals(other.seperatedValue[i])){
                    continue;
                } else {
                    int thisValue = 0;
                    int otherValue = 0;
                    var tryThisParse = int.TryParse(this.seperatedValue[i] ,out thisValue);
                    var tryOtherParse = int.TryParse(other.seperatedValue[i],out otherValue);
                    if(tryThisParse && tryOtherParse) {
                        return thisValue.CompareTo(otherValue);
                    } else if(tryThisParse) {
                        return -1;
                    } else if(tryOtherParse) {
                        return 1;
                    } else {
                        return this.seperatedValue[i].CompareTo(other.seperatedValue[i]);
                    }
                }
            }

            return this.seperatedValue.Length.CompareTo(other.seperatedValue.Length);
        }

        public override bool Equals(object obj) {
            if(obj == null) return false;

            PreReleaseField v = obj as PreReleaseField;
            
            if(v == null) return false;

            if(ReferenceEquals(this,obj)) return true;

            return this.originalValue.Equals(v.originalValue);
        }

        public override int GetHashCode()
        {
            return originalValue.GetHashCode();
        }

        public override string ToString() {
            return originalValue;
        }
    }

    #region TEST
    [TestFixture]
    public class HIVESemanticVersionTest {

        static string v100_alphaStr = "1.0.0-alpha";
        static string v100_alpha1Str = "1.0.0-alpha.1";
        static string v100_alpha_betaStr = "1.0.0-alpha.beta";
        static string v100_betaStr = "1.0.0-beta";
        static string v100_beta2Str = "1.0.0-beta.2";
        static string v100_beta11Str = "1.0.0-beta.11";
        static string v100_rc1Str = "1.0.0-rc.1";
        static string v100_rc1_200Str = "1.0.0-rc.1+200";
        static string v100Str = "1.0.0";

        HIVESemanticVersion v100_alpha = v100_alphaStr;
        HIVESemanticVersion v100_alpha1 = v100_alpha1Str;
        HIVESemanticVersion v100_alpha_beta = v100_alpha_betaStr;
        HIVESemanticVersion v100_beta = v100_betaStr;
        HIVESemanticVersion v100_beta2 = v100_beta2Str;
        HIVESemanticVersion v100_beta11 = v100_beta11Str;
        HIVESemanticVersion v100_rc1 = v100_rc1Str;
        HIVESemanticVersion v100_rc1_200 = v100_rc1_200Str;
        HIVESemanticVersion v100 = v100Str;

        [Test]
        public void ToStringTest() {
            TestContext.WriteLine("v100_alpha.ToString() : "+ v100_alpha.ToString());
            Assert.True(v100_alpha.ToString().Equals(v100_alphaStr));

            TestContext.WriteLine("v100_alpha1.ToString() : "+ v100_alpha1.ToString());
            Assert.True(v100_alpha1.ToString().Equals(v100_alpha1Str));

            TestContext.WriteLine("v100_alpha_beta.ToString() : "+ v100_alpha_beta.ToString());
            Assert.True(v100_alpha_beta.ToString().Equals(v100_alpha_betaStr));

            TestContext.WriteLine("v100_beta.ToString() : "+ v100_beta.ToString());
            Assert.True(v100_beta.ToString().Equals(v100_betaStr));

            TestContext.WriteLine("v100_beta2.ToString() : "+ v100_beta2.ToString());
            Assert.True(v100_beta2.ToString().Equals(v100_beta2Str));

            TestContext.WriteLine("v100_beta11.ToString() : "+ v100_beta11.ToString());
            Assert.True(v100_beta11.ToString().Equals(v100_beta11Str));

            TestContext.WriteLine("v100_rc1.ToString() : "+ v100_rc1.ToString());
            Assert.True(v100_rc1.ToString().Equals(v100_rc1Str));

            TestContext.WriteLine("v100_rc1_200.ToString() : "+ v100_rc1_200.ToString());
            Assert.True(v100_rc1_200.ToString().Equals(v100_rc1_200Str));

            TestContext.WriteLine("v100.ToString() : "+ v100.ToString());
            Assert.True(v100.ToString().Equals(v100Str));
        }

        [Test]
        public void VersionCompareTest() {
            TestContext.WriteLine("CompareTo 1.0.0-alpha < 1.0.0-alpha.1 < 1.0.0-alpha.beta < 1.0.0-beta < 1.0.0-beta.2 < 1.0.0-beta.11 < 1.0.0-rc.1 < 1.0.0");
            Assert.True(v100_alpha < v100_alpha1);
            Assert.True(v100_alpha < v100_alpha_beta);
            Assert.True(v100_alpha_beta < v100_beta);
            Assert.True(v100_beta < v100_beta2);
            Assert.True(v100_beta2 < v100_beta11);
            Assert.True(v100_beta11 < v100_rc1);
            Assert.False(v100_rc1 < v100_rc1_200);
            Assert.True(v100_rc1_200 < v100);

            TestContext.WriteLine("CompareTo 1.0.0-alpha < 1.0.0");
            Assert.True(v100_alpha < v100);
        }

        [Test]
        public void EqualsTest() {
            TestContext.WriteLine("v100_alpha != v100_alpha1");
            Assert.True(v100_alpha != v100_alpha1);
            TestContext.WriteLine("!v100_alpha.Equals(v100_alpha1)");
            Assert.True(!v100_alpha.Equals(v100_alpha1));

            //메타데이터는 IgnoreMetadataEquals이용시 비교되며 == 오퍼레터와 Equals는 메타데이터도 비교합니다.
            TestContext.WriteLine("v100_rc1 != v100_rc1_200");
            Assert.True(v100_rc1 != v100_rc1_200);
            TestContext.WriteLine("!v100_rc1.Equals(v100_rc1_200)");
            Assert.True(!v100_rc1.Equals(v100_rc1_200));
            TestContext.WriteLine("v100_rc1.IgnoreMetadataEquals(v100_rc1_200)");
            Assert.True(v100_rc1.IgnoreMetadataEquals(v100_rc1_200));
        }

        [Test]
        public void ArraySortTest() {
            var sortedArray = new System.Collections.Generic.List<HIVESemanticVersion>(){
                v100_alpha,
                v100_alpha1,
                v100_alpha_beta,
                v100_beta,
                v100_beta2,
                v100_beta11,
                v100_rc1,
                v100
            };
            var versions = new System.Collections.Generic.List<HIVESemanticVersion>(){
                v100_beta11,
                v100_beta,
                v100_alpha1,
                v100_alpha,
                v100_rc1,
                v100,
                v100_beta2,                
                v100_alpha_beta,
                
            };
            versions.Sort();

            for(int i=0;i<sortedArray.Count;++i) {
                TestContext.WriteLine(sortedArray[i].ToString() +"=="+versions[i].ToString());
                Assert.True(sortedArray[i] == versions[i]);
            }
        }
    }
    #endregion
}       