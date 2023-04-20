const keyUI = document.getElementById("api-key");
const keyInput = document.getElementById("txtApiKey");
const btnSubmit = document.getElementById("btnApiKey");

// Listen for submit button click
btnSubmit.addEventListener("click", (event) => {
	event.preventDefault();
	submitKey();
});

// Validate the key on input
keyInput.addEventListener("keydown", (event) => {
	if (event.key === "Enter" || event.code === "Enter") {
		event.preventDefault(); // Prevents default behavior, such as adding a new line
		submitKey();
	} else {
		updateKey();
	}
});

// Validate key when pasting into the input
keyInput.addEventListener("paste", (event) => {
	updateKey();
});

// Abstract the validation handling
const updateKey = () => {
	setTimeout(() => {
		var valid = validateKey(keyInput.value);
		btnSubmit.disabled = !valid;
		keyInput.classList.toggle('valid', valid);
		keyInput.classList.toggle('invalid', !valid);
		
		if (valid) {
			keyInput.blur();
			btnSubmit.focus();
		}
	}, 100);
}

// Send entered key to Unity
const submitKey = () => {
	console.log('JS handle submit api key');
	// Get the input value
	const apiKey = keyInput.value;
	// Send the input value to Unity
	unitySubmitApiKey(apiKey);
	// Hide the input
	keyUI.classList.toggle('hidden', true);
}

const validateKey = (apiKey) => {
    var valid = false;
    var correctLength = apiKey.length === 51;
    var isNotEmpty = apiKey !== null && apiKey !== '';

    if (isNotEmpty && correctLength) {
        var isValidSub = apiKey.substring(0, 3) === 'sk-';
        valid = isValidSub;
    }

    return valid;
}
