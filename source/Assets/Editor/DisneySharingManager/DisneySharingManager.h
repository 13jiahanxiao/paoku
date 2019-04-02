
@interface DisneySharingManager : NSObject
{
    UIActivityIndicatorView* mActivityIndicatorView;
}

+ (DisneySharingManager*)sharedManager;

- (void)showActivityIndicator;
- (void)hideActivityIndicator;


@end
