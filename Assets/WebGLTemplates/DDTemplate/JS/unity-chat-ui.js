// Define the template using a template literal
const templateMessage = `
	<div class="chat-message {{owner-class}}">
		<p>{{message}}</p>
	</div>
`;

const unityChat = document.getElementById('unity-chat');
const txtChat = document.getElementById('txtChat');

const escapeHTML = (unsafe) => {
	return unsafe
		.replace(/&/g, "&amp;")
		.replace(/</g, "&lt;")
		.replace(/>/g, "&gt;")
		.replace(/"/g, "&quot;")
		.replace(/'/g, "&#039;");
};

const AddToChat = (msg, player) => {
	console.log('JS handle chat message: ' + msg + ' from player: ' + player);

	// Escape HTML special characters and replace line breaks with <br> elements
	const formattedMsg = escapeHTML(msg).replace(/\n/g, '<br>');

	// Add the instantiated template to the DOM
	const templateContainer = document.getElementById('chat-messages');

	// Append to inner contents instead
	var newMessage = templateMessage.replace('{{message}}', formattedMsg).replace('{{owner-class}}', player ? 'player' : 'ai');
	templateContainer.innerHTML += newMessage;
};

// Show or hide the input text area
const ShowInputAction = (show) => {
	txtChat.classList.toggle('hidden', !show);
	SetChatActive(show);
	if (show) {
		txtChat.focus();
	}
}

const SetChatActive = (active) => {
	// Direct input to web UI if active, otherwise it goes to Unity
	unityChat.classList.toggle('active', active);
}

// Hide input on start
ShowInputAction(false);