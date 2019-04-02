
#import "TapjoyConnect.h"

@interface TapjoyManager : NSObject
{
    BOOL mOfferWallIsVisible;
    int mNumTapjoyPointsToDeduct;
    int mNumOzCoinsToBeAwarded; // The user can accumulate coins in the Offer Wall (they're only awarded once the offer wall is closed)
}

+ (TapjoyManager*)sharedManager;

- (void)showOffers;
- (BOOL)getOfferWallIsVisible;
- (void)initWithDeviceId:(NSString*)deviceId;
- (void)applicationDidBecomeActive;
- (void)applicationWillResignActive;
- (void)deductTapjoyCurrency:(int)amount;

@end
