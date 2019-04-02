
@interface AirshipManager : NSObject
{
    NSString* mDestinationScreen;
    NSString* mPayload;
}

+ (AirshipManager*)sharedManager;

- (NSString*)getDestinationScreen;
- (void)setDestinationScreen:(NSString*)destinationScreen;
- (NSString*)getPayload;
- (void)setPayload:(NSString*)payload;
- (void)showAlert:(NSString*)title message:(NSString*)message viewButtonText:(NSString*)viewButtonText cancelButtonText:(NSString*)cancelButtonText;

@end
