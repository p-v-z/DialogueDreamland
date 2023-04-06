const textarea = document.getElementById("txtChat");

textarea.addEventListener("keydown", (event) => {
	if (event.key === "Enter" || event.code === "Enter") {
		event.preventDefault(); // Prevents default behavior, such as adding a new line
		unitySubmitInput();
	}
});

// if the area is not focussed, web input should be disabled (so that Unity can handle input)
textarea.addEventListener("blur", (event) => {
	SetChatActive(false);
});

// if the area is focussed, web input should be enabled
textarea.addEventListener("focus", (event) => {
	SetChatActive(true);
});