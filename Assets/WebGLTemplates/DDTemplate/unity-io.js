console.log("Unity io init");

function myFunction() {
  console.log("Hello, World!");
  var unityInstance = document.unityInstance;
  if (unityInstance) {
    unityInstance.SendMessage("InterOp", "Test", "Hello from JavaScript!");
  } else {
    console.warn("Unity instance not found");
  }
}

function unityPaste() {
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