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
