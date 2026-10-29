using System;

[Serializable]
public class TextData
{
	public static string[] ANIMAL_DESCRIPTIONS;

	public static string FEATURE_TITLE_CAT;

	public static string FEATURE_TITLE_DOG;

	public static string FEATURE_TITLE_PIG;

	public static string FEATURE_TITLE_ELEPHANT;

	public static string FEATURE_TITLE_METAL;

	public static string FEATURE_TITLE_LOOP;

	public static string FEATURE_TITLE_EXTREME;

	public static string FEATURE_TITLE_HISTORY;

	public static string FEATURE_TITLE_ROCKET_CART;

	public static string FEATURE_TITLE_MAGNET_CART;

	public static string FEATURE_TITLE_HOVER_CART;

	public static string FEATURE_TITLE_CART;

	public static string FEATURE_TEXT_CAT;

	public static string FEATURE_TEXT_DOG;

	public static string FEATURE_TEXT_PIG;

	public static string FEATURE_TEXT_ELEPHANT;

	public static string FEATURE_TEXT_METAL;

	public static string FEATURE_TEXT_LOOP;

	public static string FEATURE_TEXT_EXTREME;

	public static string FEATURE_TEXT_HISTORY;

	public static string FEATURE_TEXT_ROCKET_CART;

	public static string FEATURE_TEXT_MAGNET_CART;

	public static string FEATURE_TEXT_HOVER_CART;

	public static string FEATURE_TEXT_CART;

	public static string CHRISTMAS_PACKAGE_UNLOCK_TEXT;

	public static string COMMUNITY_TITLE_FACEBOOK_LIKE;

	public static string COMMUNITY_TITLE_FACEBOOK_POST;

	public static string COMMUNITY_TITLE_TWITTER;

	public static string COMMUNITY_TITLE_EMAIL;

	public static string COMMUNITY_TITLE_TAPJOY_OFFERWALL;

	public static string COMMUNITY_TEXT_RECEIVE_COINS;

	public static string COMMUNITY_TITLE_OFFER1;

	public static string COMMUNITY_TITLE_OFFER2;

	public static string COMMUNITY_TEXT_OFFER_TWITTER;

	public static string COMMUNITY_TEXT_OFFER_POST_FACEBOOK;

	public static string COMMUNITY_TEXT_OFFER_LIKE_FACEBOOK;

	public static string COMMUNITY_TEXT_OFFER_EMAIL;

	public static string COMMUNITY_TEXT_OFFER_DOUBLE_COINS;

	public static string COMMUNITY_TEXT_OFFER_TAPJOY_OFFERWALL;

	public static string IAP_TITLE_PURCHASE_COMPLETE;

	public static string IAP_TEXT_PURCHASE_COMPLETE;

	public static string IAP_TITLE_PURCHASE_FAIL;

	public static string IAP_TEXT_PURCHASE_FAIL;

	public static string MISSION_TEXT_COMPLETED;

	public static string RESULT_TITLE_COINS_NEEDED;

	public static string RESULT_TEXT_COINS_NEEDED;

	public static string RESULT_TITLE_CARTS_NEEDED;

	public static string RESULT_TEXT_CARTS_NEEDED;

	public static string STORE_TITLE_NEED_COINS;

	public static string STORE_TEXT_NEED_COINS;

	public static string STORE_TITLE_EMPTY_CART;

	public static string STORE_TEXT_EMPTY_CART;

	public static string BUTTON_LEVEL_NEED_COINS1;

	public static string BUTTON_LEVEL_NEED_COINS2;

	public static string BUTTON_LEVEL_BUILD_NOW;

	public static string LEVELS_TITLE_NEED_COINS;

	public static string LEVELS_TEXT_NEED_COINS;

	public static string LEVELS_TITLE_LOCKED;

	public static string LEVELS_TEXT_LOCKED;

	public static string LEVELPACK_TITLE_LOCKED;

	public static string LEVELPACK_TEXT_LOCKED;

	public static string LEVELPACK_EXTRA_TEXT_LOCKED;

	public static string COINSTORE_NOTIFY_VALUE1;

	public static string COINSTORE_NOTIFY_VALUE2;

	public static string COINSTORE_NOTIFY_POPULAR1;

	public static string COINSTORE_NOTIFY_POPULAR2;

	public static string COINSTORE_NOTIFY_SAVE;

	public static string COINSTORE_NOTIFY_SALE;

	public static string COINSTORE_NOTIFY_BONUS;

	public static string COINSTORE_SALE_TITLE;

	public static string COINSTORE_DOUBLE_COINS_TITLE;

	public static string COINSTORE_DOUBLE_COINS_INFO;

	public static string REVIEW_REQUEST_TITLE;

	public static string REVIEW_REQUEST_TEXT;

	public static string REVIEW_REQUEST_CONFIRM;

	public static string REVIEW_REQUEST_DECIDE_LATER;

	public static string REVIEW_REQUEST_REJECT;

	public static string UPDATE_TITLE;

	public static string UPDATE_TEXT;

	public static string Exit_title_1;

	public static string Exit_text_2;

	public static void init()
	{
		LocalizationManager.getInstance();
		ANIMAL_DESCRIPTIONS = new string[6];
		ANIMAL_DESCRIPTIONS[0] = LocalizationManager.localize("ITEM_TEXT_MOUSE");
		ANIMAL_DESCRIPTIONS[1] = LocalizationManager.localize("ITEM_TEXT_CAT");
		ANIMAL_DESCRIPTIONS[2] = LocalizationManager.localize("ITEM_TEXT_DOG");
		ANIMAL_DESCRIPTIONS[3] = LocalizationManager.localize("ITEM_TEXT_PIG");
		ANIMAL_DESCRIPTIONS[4] = LocalizationManager.localize("ITEM_TEXT_ELEPHANT");
		ANIMAL_DESCRIPTIONS[5] = LocalizationManager.localize("ITEM_TEXT_CART");
		Exit_title_1 = LocalizationManager.localize("EXIT_TITLE_1");
		Exit_text_2 = LocalizationManager.localize("EXIT_TEXT_2");
		FEATURE_TITLE_CAT = LocalizationManager.localize("FEATURE_TITLE_CAT");
		FEATURE_TITLE_DOG = LocalizationManager.localize("FEATURE_TITLE_DOG");
		FEATURE_TITLE_PIG = LocalizationManager.localize("FEATURE_TITLE_PIG");
		FEATURE_TITLE_ELEPHANT = LocalizationManager.localize("FEATURE_TITLE_ELEPHANT");
		FEATURE_TITLE_METAL = LocalizationManager.localize("FEATURE_TITLE_METAL");
		FEATURE_TITLE_LOOP = LocalizationManager.localize("FEATURE_TITLE_LOOP");
		FEATURE_TITLE_EXTREME = LocalizationManager.localize("FEATURE_TITLE_EXTREME");
		FEATURE_TITLE_HISTORY = LocalizationManager.localize("FEATURE_TITLE_HISTORY");
		FEATURE_TITLE_ROCKET_CART = LocalizationManager.localize("FEATURE_TITLE_ROCKET_CART");
		FEATURE_TITLE_MAGNET_CART = LocalizationManager.localize("FEATURE_TITLE_MAGNET_CART");
		FEATURE_TITLE_HOVER_CART = LocalizationManager.localize("FEATURE_TITLE_HOVER_CART");
		FEATURE_TITLE_CART = LocalizationManager.localize("FEATURE_TITLE_CART");
		FEATURE_TEXT_CAT = LocalizationManager.localize("FEATURE_TEXT_CAT");
		FEATURE_TEXT_DOG = LocalizationManager.localize("FEATURE_TEXT_DOG");
		FEATURE_TEXT_PIG = LocalizationManager.localize("FEATURE_TEXT_PIG");
		FEATURE_TEXT_ELEPHANT = LocalizationManager.localize("FEATURE_TEXT_ELEPHANT");
		FEATURE_TEXT_METAL = LocalizationManager.localize("FEATURE_TEXT_METAL");
		FEATURE_TEXT_LOOP = LocalizationManager.localize("FEATURE_TEXT_LOOP");
		FEATURE_TEXT_EXTREME = LocalizationManager.localize("FEATURE_TEXT_EXTREME");
		FEATURE_TEXT_HISTORY = LocalizationManager.localize("FEATURE_TEXT_HISTORY");
		FEATURE_TEXT_ROCKET_CART = LocalizationManager.localize("FEATURE_TEXT_ROCKET_CART");
		FEATURE_TEXT_MAGNET_CART = LocalizationManager.localize("FEATURE_TEXT_MAGNET_CART");
		FEATURE_TEXT_HOVER_CART = LocalizationManager.localize("FEATURE_TEXT_HOVER_CART");
		FEATURE_TEXT_CART = LocalizationManager.localize("FEATURE_TEXT_CART");
		CHRISTMAS_PACKAGE_UNLOCK_TEXT = LocalizationManager.localize("CHRISTMAS_PACKAGE_UNLOCK_TEXT");
		COMMUNITY_TITLE_FACEBOOK_LIKE = LocalizationManager.localize("COMMUNITY_TITLE_FACEBOOK_LIKE");
		COMMUNITY_TITLE_FACEBOOK_POST = LocalizationManager.localize("COMMUNITY_TITLE_FACEBOOK_POST");
		COMMUNITY_TITLE_TWITTER = LocalizationManager.localize("COMMUNITY_TITLE_TWITTER");
		COMMUNITY_TITLE_EMAIL = LocalizationManager.localize("COMMUNITY_TITLE_EMAIL");
		COMMUNITY_TEXT_RECEIVE_COINS = LocalizationManager.localize("COMMUNITY_TEXT_RECEIVE_COINS");
		COMMUNITY_TITLE_OFFER1 = LocalizationManager.localize("COMMUNITY_TITLE_OFFER1");
		COMMUNITY_TITLE_OFFER2 = LocalizationManager.localize("COMMUNITY_TITLE_OFFER2");
		COMMUNITY_TITLE_TAPJOY_OFFERWALL = LocalizationManager.localize("TAPJOY_1");
		COMMUNITY_TEXT_OFFER_TWITTER = LocalizationManager.localize("COMMUNITY_TEXT_OFFER_TWITTER");
		COMMUNITY_TEXT_OFFER_POST_FACEBOOK = LocalizationManager.localize("COMMUNITY_TEXT_OFFER_POST_FACEBOOK");
		COMMUNITY_TEXT_OFFER_LIKE_FACEBOOK = LocalizationManager.localize("COMMUNITY_TEXT_OFFER_LIKE_FACEBOOK");
		COMMUNITY_TEXT_OFFER_EMAIL = LocalizationManager.localize("COMMUNITY_TEXT_OFFER_EMAIL");
		COMMUNITY_TEXT_OFFER_DOUBLE_COINS = LocalizationManager.localize("COMMUNITY_TEXT_OFFER_DOUBLE_COINS");
		COMMUNITY_TEXT_OFFER_TAPJOY_OFFERWALL = LocalizationManager.localize("TAPJOY_2");
		IAP_TITLE_PURCHASE_COMPLETE = LocalizationManager.localize("IAP_TITLE_PURCHASE_COMPLETE");
		IAP_TEXT_PURCHASE_COMPLETE = LocalizationManager.localize("IAP_TEXT_PURCHASE_COMPLETE");
		IAP_TITLE_PURCHASE_FAIL = LocalizationManager.localize("IAP_TITLE_PURCHASE_FAIL");
		IAP_TEXT_PURCHASE_FAIL = LocalizationManager.localize("IAP_TEXT_PURCHASE_FAIL");
		MISSION_TEXT_COMPLETED = LocalizationManager.localize("MISSION_TEXT_COMPLETED");
		RESULT_TITLE_COINS_NEEDED = LocalizationManager.localize("RESULT_TITLE_COINS_NEEDED");
		RESULT_TEXT_COINS_NEEDED = LocalizationManager.localize("RESULT_TEXT_COINS_NEEDED");
		RESULT_TITLE_CARTS_NEEDED = LocalizationManager.localize("RESULT_TITLE_CARTS_NEEDED");
		RESULT_TEXT_CARTS_NEEDED = LocalizationManager.localize("RESULT_TEXT_CARTS_NEEDED");
		STORE_TITLE_NEED_COINS = LocalizationManager.localize("STORE_TITLE_NEED_COINS");
		STORE_TEXT_NEED_COINS = LocalizationManager.localize("STORE_TEXT_NEED_COINS");
		STORE_TITLE_EMPTY_CART = LocalizationManager.localize("STORE_TITLE_EMPTY_CART");
		STORE_TEXT_EMPTY_CART = LocalizationManager.localize("STORE_TEXT_EMPTY_CART");
		BUTTON_LEVEL_NEED_COINS1 = LocalizationManager.localize("BUTTON_LEVEL_NEED_COINS1");
		BUTTON_LEVEL_NEED_COINS2 = LocalizationManager.localize("BUTTON_LEVEL_NEED_COINS2");
		BUTTON_LEVEL_BUILD_NOW = LocalizationManager.localize("BUTTON_LEVEL_BUILD_NOW");
		LEVELS_TITLE_NEED_COINS = LocalizationManager.localize("LEVELS_TITLE_NEED_COINS");
		LEVELS_TEXT_NEED_COINS = LocalizationManager.localize("LEVELS_TEXT_NEED_COINS");
		LEVELS_TITLE_LOCKED = LocalizationManager.localize("LEVELS_TITLE_LOCKED");
		LEVELS_TEXT_LOCKED = LocalizationManager.localize("LEVELS_TEXT_LOCKED");
		LEVELPACK_TITLE_LOCKED = LocalizationManager.localize("LEVELPACK_TITLE_LOCKED");
		LEVELPACK_TEXT_LOCKED = LocalizationManager.localize("LEVELPACK_TEXT_LOCKED");
		LEVELPACK_EXTRA_TEXT_LOCKED = LocalizationManager.localize("LEVELPACK_EXTRA_TEXT_LOCKED");
		COINSTORE_NOTIFY_VALUE1 = LocalizationManager.localize("COINSTORE_NOTIFY_VALUE_1");
		COINSTORE_NOTIFY_VALUE2 = LocalizationManager.localize("COINSTORE_NOTIFY_VALUE_2");
		COINSTORE_NOTIFY_POPULAR1 = LocalizationManager.localize("COINSTORE_NOTIFY_POPULAR_1");
		COINSTORE_NOTIFY_POPULAR2 = LocalizationManager.localize("COINSTORE_NOTIFY_POPULAR_2");
		COINSTORE_NOTIFY_SAVE = LocalizationManager.localize("COINSTORE_NOTIFY_SAVE");
		COINSTORE_NOTIFY_SALE = LocalizationManager.localize("COINSTORE_NOTIFY_SALE");
		COINSTORE_NOTIFY_BONUS = LocalizationManager.localize("COINSTORE_NOTIFY_BONUS");
		COINSTORE_SALE_TITLE = LocalizationManager.localize("COINSTORE_SALE_TITLE");
		COINSTORE_DOUBLE_COINS_TITLE = LocalizationManager.localize("COINSTORE_DOUBLE_COINS_TITLE");
		COINSTORE_DOUBLE_COINS_INFO = LocalizationManager.localize("COINSTORE_DOUBLE_COINS_INFO");
		REVIEW_REQUEST_TITLE = LocalizationManager.localize("REVIEW_REQUEST_TITLE");
		REVIEW_REQUEST_TEXT = LocalizationManager.localize("REVIEW_REQUEST_TEXT");
		REVIEW_REQUEST_CONFIRM = LocalizationManager.localize("REVIEW_REQUEST_CONFIRM");
		REVIEW_REQUEST_DECIDE_LATER = LocalizationManager.localize("REVIEW_REQUEST_DECIDE_LATER");
		REVIEW_REQUEST_REJECT = LocalizationManager.localize("REVIEW_REQUEST_REJECT");
		UPDATE_TITLE = LocalizationManager.localize("UPDATE_TITLE");
		UPDATE_TEXT = LocalizationManager.localize("UPDATE_TEXT");
	}
}
