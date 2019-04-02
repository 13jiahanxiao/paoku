//
//  ReferralStoreBinding.m
//
//  Created by Josh Noble on 01/30/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "ReferralStoreManager.h"


void _showReferralStore()
{
    [[ReferralStoreManager sharedManager] show];
}

bool _getReferralStoreIsOpen()
{
    return [ReferralStoreManager isStoreOpen];
}
