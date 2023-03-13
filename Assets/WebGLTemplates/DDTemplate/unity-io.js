// Init
console.log("Initializing unity-io.js");
const getUnityInstance = () => {
	if (unityInstance == null) {
		unityInstance = document.unityInstance;
	} 
	return unityInstance;
}
var unityInstance = getUnityInstance();

const unityPaste = () => {
	console.log("Trying to paste clipboard to Unity");

	navigator.clipboard.readText()
	.then((clipboardText) => {
		console.log("Clipboard content:", clipboardText);
		getUnityInstance().SendMessage("InterOp", "Paste", clipboardText);
	})
	.catch((err) => {
		console.error("Failed to read clipboard contents: ", err);
	});
}

const unitySubmitInput = () => {
	var input = document.getElementById("txtChat");
	console.log("Submit input to Unity, value: " + input.value);
	getUnityInstance().SendMessage("InterOp", "SubmitChatMessage", input.value);
}