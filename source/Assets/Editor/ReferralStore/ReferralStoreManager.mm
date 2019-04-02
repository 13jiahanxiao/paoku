//
//  ReferralStoreManager.mm
//
//  Created by Josh Noble on 01/30/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "ReferralStoreManager.h"

void UnityPause( bool pause );
UIViewController *UnityGetGLViewController();



@implementation ReferralStoreManager

static bool isOpen = false;

+ (ReferralStoreManager*)sharedManager
{
	static ReferralStoreManager* sharedSingleton;
	
	if ( !sharedSingleton )
    {
		sharedSingleton = [[ReferralStoreManager alloc] init];
        isOpen = false;
	}
    
	return sharedSingleton;
}

+ (bool) isStoreOpen
{
    return isOpen;
}

- (void)show
{
    isOpen = true;
    
    UnityPause( true );
    
    mReferralStoreVC = [[DMNReferralStoreViewController alloc] init];
    mReferralStoreVC.delegate = self;
    
    // Push the MoreGames ViewController onto the NavigationController stack
    UIViewController* vc = UnityGetGLViewController();
    //[vc.navigationController pushViewController:mReferralStoreVC animated:YES];
    [vc presentModalViewController:mReferralStoreVC animated:YES];
    
    // Release view controller
    mReferralStoreVC = nil;
}

// --- Delegate methods --------------------------------------------------------

- (void)referralStoreViewControllerDidFinish:(DMNReferralStoreViewController*)referralStoreViewController
{
    isOpen = false;
    
    UnityPause( false );
    
    UnitySendMessage("GameController", "UpdateIpodMusicIsOn","");

    UIViewController* vc = UnityGetGLViewController();
    //[vc.navigationController popViewControllerAnimated:YES];
    [vc dismissModalViewControllerAnimated:YES];
}

- (void)dealloc
{
    mReferralStoreVC.delegate = nil;
    
    [super dealloc];
}

@end

