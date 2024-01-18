//
//  HIVEAppController.m
//  Unity-iPhone
//
//  Created by paikjongman on 2016. 12. 9..
//
//

#import <WebKit/WebKit.h>
#import <UIKit/UIKit.h>
#import <GameKit/GameKit.h>
#import <Foundation/Foundation.h>

//#import <HIVECore/HIVECore-Swift.h>
//#import <ProviderAdapter/ProviderAdapter-Swift.h>
//#import <HIVEService/HIVEService-Swift.h>

#import "UnityAppController.h"

@interface HIVEAppController<HIVEDelegate> : UnityAppController
- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(nullable NSDictionary *)launchOptions;
@end

@implementation HIVEAppController
- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(nullable NSDictionary *)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    Class clzHIVEAppDelegate = NSClassFromString(@"HIVEAppDelegate");
    SEL selApplicationDidFinishLaunchingWithOptions = NSSelectorFromString(@"application:didFinishLaunchingWithOptions:");
    if( clzHIVEAppDelegate != nil && [clzHIVEAppDelegate respondsToSelector:selApplicationDidFinishLaunchingWithOptions] ) {
        NSMethodSignature *method = [clzHIVEAppDelegate methodSignatureForSelector:selApplicationDidFinishLaunchingWithOptions];
        if (method != nil) {
            
            NSInvocation *invocation = [NSInvocation invocationWithMethodSignature:method];
            [invocation setSelector:selApplicationDidFinishLaunchingWithOptions];
            [invocation setTarget:clzHIVEAppDelegate];
            [invocation setArgument:(void*)&application atIndex:2];
            if( launchOptions != nil )
                [invocation setArgument:(void*)&launchOptions atIndex:3];
            [invocation invoke];
            
            BOOL returnBoolValue = FALSE;
            [invocation getReturnValue:&returnBoolValue];

            return returnBoolValue;
        }
    }
    return YES;
//    return [HIVEAppDelegate application:application didFinishLaunchingWithOptions:launchOptions];
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(HIVEAppController)
