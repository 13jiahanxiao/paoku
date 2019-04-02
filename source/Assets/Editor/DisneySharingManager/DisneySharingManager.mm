//
//  DisneySharingManager.mm
//
//  Created by Josh Noble on 02/10/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "DisneySharingManager.h"


UIViewController *UnityGetGLViewController();


@implementation DisneySharingManager

+ (DisneySharingManager*)sharedManager
{
	static DisneySharingManager* sharedSingleton;
	
	if ( !sharedSingleton )
    {
		sharedSingleton = [[DisneySharingManager alloc] init];
	}
    
	return sharedSingleton;
}

- (void)showActivityIndicator
{
    [self hideActivityIndicator];
    
    mActivityIndicatorView = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
    
    CGRect viewFrame = UnityGetGLViewController().view.frame;
    
    mActivityIndicatorView.center = CGPointMake( ( viewFrame.size.width / 2 ), ( viewFrame.size.height / 2 ) );
    mActivityIndicatorView.hidesWhenStopped = YES;
    
    [UnityGetGLViewController().view addSubview:mActivityIndicatorView];
    
    [mActivityIndicatorView startAnimating];
}

- (void)hideActivityIndicator
{
    if ( mActivityIndicatorView != nil )
    {
        [mActivityIndicatorView stopAnimating];
        [mActivityIndicatorView release];
        mActivityIndicatorView = nil;
    }
}

- (void)dealloc
{
    if ( mActivityIndicatorView != nil )
    {
        [mActivityIndicatorView stopAnimating];
        [mActivityIndicatorView release];
        mActivityIndicatorView = nil;
    }
    
    [super dealloc];
}

@end

