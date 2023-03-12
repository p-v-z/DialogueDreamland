mergeInto(LibraryManager.library, {
	AddChatJS: function (msg, player) {
		console.log("Add HTML dialogue from Unity");
		console.log("str: " + msg);
		console.log("player: " + player);

		// get unity-chat-ui.js 
		var utfString = UTF8ToString(msg);
		var unityInstance = document.unityInstance;
		if (unityInstance) {
			AddToChat(utfString, player);
		}
	},
	ShowInputJS: function (action) {
	    console.log("Show input from Unity " + action);
	    ShowInputAction(action);
	}
});
