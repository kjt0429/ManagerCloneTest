//
//  NotificationService.m
//  NotificationServiceExtension
//
//  Created by joungdaun on 09/05/2019.
//

#import "NotificationService.h"
#import <HIVEExtensions/HIVEExtensions.h>
@interface NotificationService ()

@property (nonatomic, strong) void (^contentHandler)(UNNotificationContent *contentToDeliver);
@property (nonatomic, strong) UNMutableNotificationContent *bestAttemptContent;

@end

@implementation NotificationService

- (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent * _Nonnull))contentHandler {
    [HIVENotificationService didReceiveNotificationRequest:request withContentHandler:contentHandler];
}

- (void)serviceExtensionTimeWillExpire {
    [HIVENotificationService serviceExtensionTimeWillExpire];
}


@end
