//
//  DMOPromoViewController.h
//  Promoter
//
//  Created by Michael VanLandingham on 5/24/11.
//  Copyright 2011 Tapulous, Inc. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <QuartzCore/QuartzCore.h>

/*!
	@header DMOPromoViewController
	@abstract Controller for a set of views representing an interstitial. 
	@discussion Provides wrapper methods to customize views and their behavior, as well as loading functionality and a delegate protocol.  
 */



typedef enum {
	DMOPromoViewButtonPosition_Origin = 0,
	DMOPromoViewButtonPosition_TopRight,
	DMOPromoViewButtonPosition_BottomLeft,
	DMOPromoViewButtonPosition_BottomRight,

} DMOPromoViewButtonPosition;


/*! 
	@protocol DMOPromoViewControllerDelegate
	@abstract Interesting events from the promo view controller.
 */
@protocol DMOPromoViewControllerDelegate <NSObject>

@optional

/*!
	@method isInterfaceOrientationSupported:
	@abstract implement to limit orientation (called in shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation here)
 */
- (BOOL)isInterfaceOrientationSupported:(UIInterfaceOrientation)orientation;

/*! 
	@method didDismissInterstitial:(UIViewController*)vc
	@abstract Implement to catch dismissal of interstitial views.
 */
- (void)didDismissInterstitial:(UIViewController*)vc;


/*! 
 @method viewController:wantsOpenURL:
 @abstract Implement to catch URL request that would open an external URL.. delegate should dismiss the view and handle the URL.
 */
- (void)viewController:(UIViewController*)vc wantsOpenURL:(NSURL*)url;

/*! 
 @method htmlDidFinishLoading
 @abstract Called when the webView is finished loading (via UIWebViewDelegate method webViewDidFinishLoad:)
 */
- (void)htmlDidFinishLoading;

@end



/*! 
	@class DMOPromoViewController
	@abstract Manage a set of views for displaying interstitial and / or sponsor.
	@discussion This works best when you use it with your app's existing view controller, and calling "presentModalViewController" with a setup DMOPromoViewController. Initialize it, present it and load the content.
	
 */

@interface DMOPromoViewController : UIViewController <UIWebViewDelegate> {
	id				_delegate;
	UIWebView*		_webView;
}

/*! 
	@method initWithDelegate:
	@abstract Designated initializer.
	@parameter An object which implements the DMOPromoViewControllerDelegate protocol (above). Ideally this should be the parent UIViewController which will present the interstitial.
 */

- (DMOPromoViewController*)initWithDelegate:(id<DMOPromoViewControllerDelegate>) delegate;

/*! 
	@property delegate
	@abstract Set this and implement the DMOPromoViewControllerDelegate protocol, below. 
 */
@property (nonatomic, readwrite, assign) id delegate;


@property (nonatomic, readwrite, assign) BOOL useFadeIn;
@property (nonatomic, readwrite, assign) BOOL useFadeOut;

/*! 
	@property webView
	@abstract This is the view that loads any HTML from an interstitial. 
 */
@property (nonatomic, readwrite, retain) UIWebView* webView;


/*! 
 @property useBorder
 @abstract Use an outline around the view. 
 */
@property (nonatomic, readwrite, assign) BOOL useBorder;
// border properties:
@property (nonatomic, readwrite, assign) CGFloat  borderWidth;
@property (nonatomic, readwrite, assign) CGFloat  borderCornerRadius;
@property (nonatomic, readwrite, retain) UIColor* borderColor;


/*! 
	@property closeButton
	@abstract The button that is used to dismiss an interstitial. 
 */
@property (nonatomic, readwrite, retain) UIButton* closeButton;

// button properties:
@property (nonatomic, readwrite, assign) CGSize	  buttonSize;
@property (nonatomic, readwrite, retain) UIColor* buttonFillColor;
@property (nonatomic, readwrite, retain) UIColor* buttonStrokeColor;

/*! 
	@property buttonPosition
	@abstract The position of the button that is used to dismiss an interstitial. Uses the DMOPromoViewButtonPosition enum, above.
 */
@property (nonatomic, readwrite, assign) DMOPromoViewButtonPosition buttonPosition;


/*! 
	@method loadInterstitial:
	@param NSString* html, a string of html to display.
	@abstract Convenience wrapper, calls the underlying webview's loadHTMLString:baseURL: method.
 */
- (void)loadInterstitial:(NSString*)html;

@end

