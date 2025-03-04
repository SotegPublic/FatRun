mergeInto(LibraryManager.library, {

	RateGame: function () 
	{
		ysdk.feedback.canReview()
		.then(({ value, reason }) => 
		{
			if (value) 
			{
				ysdk.feedback.requestReview()
					.then(({ feedbackSent }) => 
					{
						console.log(feedbackSent);
					})
			} 
			else 
			{
				console.log(reason)
			}
		})
	},
	
	SaveExtern: function (date) 
	{
		var dateString = UTF8ToString(date);
		var myObj = JSON.parse(dateString);
		player.setData(myObj);
		
		console.log('data saved');
	},
	
	LoadExtern: function () 
	{
		player.getData().then(_date => {
			const myJSON = JSON.stringify(_date);
			myGameInstance.SendMessage('PlayerGameModel', 'LoadPlayerData', myJSON);
			console.log('myJSON');
		});
	},
	
	GetDeviceType: function () 
	{
		var data = ysdk.deviceInfo;
		var divType = data.type;
		var bufferSize = lengthBytesUTF8(divType) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(divType, buffer, bufferSize);
		console.log(buffer);
		return buffer;
	},
	
	GetLanguage: function () 
	{
		var lang = ysdk.environment.i18n.lang;
		var bufferSize = lengthBytesUTF8(lang) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(lang, buffer, bufferSize);
		console.log('load language');
		console.log(buffer);
		return buffer;
	},
	
	Loaded: function (language, device) 
	{
		var lang = UTF8ToString(language);
		var dev = UTF8ToString(device);
		console.log('loaded');
		console.log(lang);
		console.log(dev);
	},
	
	SendReady: function()
	{
		if (ysdk !== null && ysdk.features.LoadingAPI !== undefined && ysdk.features.LoadingAPI !== null && initGame !== true) {
			ysdk.features.LoadingAPI.ready();
			initGame = true;
			console.log('Game Ready');
		}
	},
	
	ShowAdvBetweenLevels: function () 
	{
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function(wasShown) {
					myGameInstance.SendMessage('PlayerGameModel', 'ReturnSound');
				},
				onError: function(error) {
					myGameInstance.SendMessage('PlayerGameModel', 'ReturnSound');
				}
			}
		})
	},
	
	ShowAdvBeforeMainMenu: function () 
	{
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function(wasShown) {
					myGameInstance.SendMessage('PlayerGameModel', 'AdvBeforeMainMenuClosed');
				},
				onError: function(error) {
					myGameInstance.SendMessage('PlayerGameModel', 'AdvBeforeMainMenuClosed');
				}
			}
		})
	},
	
	ShowDoubleAdv: function (value) 
	{
		ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
				  console.log('Video ad open.');
				},
				onRewarded: () => {
					myGameInstance.SendMessage('PlayerGameModel', 'DoubleReward', value);
				  console.log('Rewarded!');
				},
				onClose: () => {
					myGameInstance.SendMessage('PlayerGameModel', 'WhenRewardVideoClosed');
				  console.log('Video ad closed.');
				},
				onError: (e) => {
					myGameInstance.SendMessage('PlayerGameModel', 'WhenRewardVideoClosed');
				  console.log('Error while open video ad:', e);
				}
			}
		})
	},
	
	CheckUnprocessedPurchases: function () 
	{
		console.log('start check unprocessed purchases');
		
		payments.getPurchases().then
		(
			purchases => purchases.forEach
			(
				purchase =>
				{	
				
					function consPurchase(purchase, method)
					{
						return new Promise((resolve, reject) =>
						{
							myGameInstance.SendMessage('PlayerGameModel', method);
							console.log(method);
							resolve(purchase);
						})
					};
				
					if (purchase.productID === 'speedUp')
					{				
						consPurchase(purchase, 'UpSpeed').then(purchase =>				
						{
							payments.consumePurchase(purchase.purchaseToken);
							console.log('unprocessed up');
						})					
					}
					
					if (purchase.productID === 'slowDown')
					{
						consPurchase(purchase, 'DownSpeed').then(purchase =>				
						{
							payments.consumePurchase(purchase.purchaseToken);
							console.log('unprocessed down');
						})	
					}
					
					if (purchase.productID === 'speedUpx10')
					{
						consPurchase(purchase, 'UpSpeedX5').then(purchase =>				
						{
							payments.consumePurchase(purchase.purchaseToken);
							console.log('unprocessed up x5');
						})	
					}
					
					if (purchase.productID === 'slowDownx10')
					{
						consPurchase(purchase, 'DownSpeedX5').then(purchase =>				
						{
							payments.consumePurchase(purchase.purchaseToken);
							console.log('unprocessed down x5');
						})	
					}
					
					if (purchase.productID === 'fatCoins1000')
					{
						consPurchase(purchase, 'Get1000FatCoins').then(purchase =>				
						{
							payments.consumePurchase(purchase.purchaseToken);
							console.log('unprocessed coins');
						})	
					}
				}
			)
		)
		.catch(
			err => 
			{
				console.log('getPurchases catch');
			}
		);
		
		console.log('end check unprocessed purchases');
	},

	
	BuySpeedUp: function () 
	{
		payments.purchase({ id: 'speedUp' }).then(purchase => 
		{
			myGameInstance.SendMessage('PlayerGameModel', 'UpSpeed');
			return purchase;
		})
		.then( purchase => 	
			{
				payments.consumePurchase(purchase.purchaseToken);
				console.log('speed was uped');
				
				window.focus();
				canvas.focus();
				console.log('back focus');
			}
		)
		.catch(
			err => 
			{

			}
		)
	},
	
	BuySlowDown: function () 
	{
		payments.purchase({ id: 'slowDown' }).then(purchase => 
		{
			myGameInstance.SendMessage('PlayerGameModel', 'DownSpeed');
			return purchase;
		}).
		then( purchase => 	
		{
			payments.consumePurchase(purchase.purchaseToken);
			console.log('speed was slowed');
			
			window.focus();
			canvas.focus();
			console.log('back focus');
		})
		.catch(
		err => 
			{

			}
		)
	},
	
	BuySpeedUpX10: function () 
	{
		payments.purchase({ id: 'speedUpx10' }).then(purchase => 
		{
			myGameInstance.SendMessage('PlayerGameModel', 'UpSpeedX5');
			return purchase;
		})
		.then( purchase => 	
			{
				payments.consumePurchase(purchase.purchaseToken);
				console.log('speed was uped x5');
				
				window.focus();
				canvas.focus();
				console.log('back focus');
			}
		)
		.catch(
			err => 
			{

			}
		)
	},
	
	BuySlowDownX10: function () 
	{
		payments.purchase({ id: 'slowDownx10' }).then(purchase => 
		{
			myGameInstance.SendMessage('PlayerGameModel', 'DownSpeedX5');
			return purchase;
		}).
		then( purchase => 	
		{
			payments.consumePurchase(purchase.purchaseToken);
			console.log('speed was slowed x5');
			
			window.focus();
			canvas.focus();
			console.log('back focus');
		})
		.catch(
		err => 
			{

			}
		)
	},
	
	Buy1000FatCoins: function () 
	{
		payments.purchase({ id: 'fatCoins1000' }).then(purchase => 
		{
			myGameInstance.SendMessage('PlayerGameModel', 'Get1000FatCoins');
			return purchase;
		}).
		then( purchase => 	
		{
			payments.consumePurchase(purchase.purchaseToken);
			console.log('add coins');
			
			window.focus();
			canvas.focus();
			console.log('back focus');
		})
		.catch(
		err => 
			{

			}
		)
	},
	
	SetLeaderboardInfo: function (winstreak)
	{
		ysdk.getLeaderboards()
			.then(lb => {
				lb.setLeaderboardScore('winstreaks', winstreak);
		    }
		);
		
		console.log('set winstreack');
	},
	
	SetFocus: function()
	{
		window.focus();
        canvas.focus();
		console.log('back focus');
	},
	
	CheckYandexSDK: function()
	{
		if (ysdk !== null) 
		{
			console.log('yasdk loaded');
			return 1;
		}
		else
		{
			console.log('yasdk not loaded');
			return 0;
		}
	},
	
	CheckAuth: function()
	{
		initPlayer().then(_player => {
			if (_player.getMode() === 'lite') 
			{
				console.log('auth not');
				myGameInstance.SendMessage('PlayerGameModel', 'AfterAuthCheck', 0);
			}
			else 
			{
				console.log('auth done');
				myGameInstance.SendMessage('PlayerGameModel', 'AfterAuthCheck', 1);
			}
		}).catch(err => {
			console.log('error when player init');
		});
	},
	
	
	YaAuth: function()
	{
		if (player.getMode() === 'lite') 
		{
			ysdk.auth.openAuthDialog().then(() => 
			{
				console.log('auth done');
				initPlayer()
				.then(_player =>
				{
					console.log('auth player init');
					myGameInstance.SendMessage('PlayerGameModel', 'AfterAuthCheck', 1);
				})
				.catch(err => {
					console.log('error when player init');
				});
			}).catch(() => {
				console.log('auth not');
				myGameInstance.SendMessage('PlayerGameModel', 'AfterAuthCheck', 0);
			});
		}
	},
	
	SaveToLocalStorage : function(key, value) {
		console.log('saveToLocal');
		
		try {
			localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
		}
		catch (e) {
			console.error('Save to Local Storage error: ', e.message);
		}
	},

	LoadFromLocalStorage : function(key) {
		console.log('loadFromLocal');
		var returnStr = localStorage.getItem(UTF8ToString(key));
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
	
	HasKey : function(key) {
		try 
		{
			if (localStorage.getItem(UTF8ToString(key))) {
			  return 1;
			}
			else {
			  return 0;
			}
		}
		catch (e) {
			console.error('Has key in Local Storage error: ', e.message);
			return 0;
		}
	},
	
	GetProductCost : function(ID) {
	
		var productID = UTF8ToString(ID);
		
		for(var i = 0; i < gameShop.length; i++)
		{
			if(gameShop[i].id === productID)
			{
				console.log(gameShop[i].priceValue);
				
				var text = gameShop[i].priceValue;
				var bufferSize = lengthBytesUTF8(text) + 1;
				var buffer = _malloc(bufferSize);
				stringToUTF8(text, buffer, bufferSize);
				
				return buffer;
			}
		}
		
		console.log('product not found');
		return '';
	},
	
	GetCurrencyCode : function(ID) {
	
		var productID = UTF8ToString(ID);
		
		for(var i = 0; i < gameShop.length; i++)
		{
			if(gameShop[i].id === productID)
			{
				console.log(gameShop[i].priceCurrencyCode);
				
				var text = gameShop[i].priceCurrencyCode;
				var bufferSize = lengthBytesUTF8(text) + 1;
				var buffer = _malloc(bufferSize);
				stringToUTF8(text, buffer, bufferSize);
				
				return buffer;
			}
		}
		
		console.log('product not found');
		return '';
	},
	
	GetPortalCurrencySpriteUrl : function(ID) {
		var productID = UTF8ToString(ID);
		
		for(var i = 0; i < gameShop.length; i++)
		{
			if(gameShop[i].id === productID)
			{
				var url = gameShop[i].getPriceCurrencyImage('medium');
				console.log(url);
				
				var bufferSize = lengthBytesUTF8(url) + 1;
				var buffer = _malloc(bufferSize);
				stringToUTF8(url, buffer, bufferSize);
				
				return buffer;
			}
		}
		
		console.log('url not found');
		return '';
	},
		
});