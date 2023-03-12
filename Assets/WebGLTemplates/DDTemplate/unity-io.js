const unityPaste = () => {
	console.log("Trying to paste clipboard to Unity");

	navigator.clipboard.readText()
	.then((clipboardText) => {
		console.log("Clipboard content:", clipboardText);
		var unityInstance = document.unityInstance;
		unityInstance.SendMessage("InterOp", "Paste", clipboardText);
	})
	.catch((err) => {
		console.error("Failed to read clipboard contents: ", err);
	});
}