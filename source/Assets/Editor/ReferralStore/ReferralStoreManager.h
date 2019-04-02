
#import "DMNReferralStoreViewController.h"


@interface ReferralStoreManager : NSObject <DMNReferralStoreViewControllerDelegate,DMNReferralStoreConnectionDelegate>
{
    DMNReferralStoreViewController* mReferralStoreVC;
}

+ (ReferralStoreManager*)sharedManager;

+ (bool)isStoreOpen;

- (void)show;


@end
