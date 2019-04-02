//
//  DMOPromoter.h
//  Promoter
//
//  Created by Michael VanLandingham on 11/4/10.
//  Copyright 2010 Tapulous, Inc. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIDevice.h>
#import <UIKit/UIApplication.h>

typedef enum {
	DMOInterstitialType_Sponsor = 0,
	DMOInterstitialType_Boot,
	DMOInterstitialType_Post,
	DMOInterstitialType_3,
	DMOInterstitialType_4,
	DMOInterstitialType_5,
	DMOInterstitialType_6,
	DMOInterstitialType_7,
	DMOInterstitialType_8,
	DMOInterstitialType_9,
} DMOInterstitialType;


/*!
	@protocol DMOPromoterProtocol
	@abstract Implement the "interstitialRequest.." methods to receive the interstitial html code for display
		You can use a standard UIWebView or custom class to display it, or take advantage of the 
		DMOPromoViewController and/or DMOPromoView to host the interstital's html. 
 */
@protocol DMOPromoterProtocol <NSObject>
@optional


/*!
	@method
	@abstract Called on delegate when the async request for an interstitial is returned from the server.
	@discussion This is only called if there is data to show.
 */
- (void)interstitialRequestSucceeded:(NSString*)html;


/*!
 @method
 @abstract Called on delegate when the async request for an interstitial is returned from the server.
 @discussion This is called when there is no data to show.
 */

- (void)interstitialRequestSuceededNoData;


/*!
 @method
 @abstract Called on delegate when the async request for an interstitial is returned from the server.
 @discussion This is only called if there is data to show. This method was added for Analytics tracking 
	of interstitial impressions, so we can log the type.
 */
- (void)interstitialRequestOfType:(DMOInterstitialType)aType succeeded:(NSString*)html;



/*!
	@method
	@abstract Called on delegate when the async request for an interstitial encounters a failure from the server.
 */
- (void)interstitialRequestDidFailWithError:(NSError*)err;


/*!
	@method
	@abstract Called on the delegate when the sponsor screen is dismissed.
	@discussion This is the "I'm done" response to the method "presentSponsorInView:forInterval:". Use this to continue loading and/or setting up the apps main views.
 */
- (void)sponsorComplete:(BOOL)shown;


/*!
	@method
	@abstract Called on the delegate right before the sponsor screen will be removed from it's parent view.  
	@discussion This is an optional method that might allow the app to do setup _before_ the sponsor screen is dismissed.
 */
- (void)sponsorWillBeRemovedFromView:(UIView*)parentView;
//- (UIInterfaceOrientation)preferredOrientation;	// currently disabled. please handle in application code for now.
@end



@class DMOBackendConnection;

/*!	
	@class DMOPromoter
	@abstract main controller for fetching & managing interstitials
	@discussion
 */
@interface DMOPromoter : NSObject {
	
	id delegate;
	BOOL _requestSucceededWithNoData;
	
}


/*!
	@property delegate
	@abstract Assign the class instance which implements the DMOPromoterProtocol to the delegate property.
 */
@property (nonatomic, assign, readwrite) id<DMOPromoterProtocol> delegate;


/*!
	@property requestSucceededWithNoData
	@abstract convenience property for checking for the absence of interstitial data.
 */
@property (nonatomic, readonly) BOOL requestSucceededWithNoData;


/*!
	@method
	@abstract Designated initializer.  Requires key and secret parameters, uses default endpoint.
 */
- (id) initWithKey:(NSString*)key secret:(NSString*)secret;


/*!
 @method
 @abstract Designated initializer.  Requires key, secret and endpoint parameters.
 @discussion This is useful if you need to override the default endpoint, such as for non-production staging or testing builds.
  */
- (id) initWithKey:(NSString*)key secret:(NSString*)secret endpoint:(NSString*)endpoint;


/*!
	@method requestInterstitial:(DMOInterstitialType)type
	@abstract Request an interstitial or sponsor resource from the server.  Implement delegate protocol methods for receiving the interstitial.  
	A sponsor image is treated differently, being cached and displayed on subsequent launches.
 */
- (void) requestInterstitial:(DMOInterstitialType)interType;


/*!
	@method presentSponsorInView:forInterval
	@abstract Show a sposor image, if it exists, hosted in the provided parent view, for some number of seconds.
	@discussion When it's done, it calls the delegate protocol method "sponsorComplete:"
 */
- (void) presentSponsorInView:(UIView*)parentView forInterval:(NSTimeInterval)seconds;


/*!
	@method sponsorImage
	@abstract returns a sponsor image if there is one cached.  If nil, no sponsor exists.  Caller is responsible for showing the image.
 */
- (UIImage*)sponsorImage;

@end


