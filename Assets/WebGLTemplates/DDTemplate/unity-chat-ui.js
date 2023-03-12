// Define the template using a template literal
const templateMessage = `
    <div class="chat-message {{owner-class}}">
	    <p>{{message}}</p>
    </div>
`;

// Rewrite Test() to use modern syntax
const AddToChat = (msg, player) => {
	console.log('JS handle chat message');
	console.log('input:', msg);
	console.log('player:', player);
	
	// Add the instantiated template to the DOM
	const templateContainer = document.getElementById('unity-chat');
	// Append to inner contents instead
	var newMessage = templateMessage.replace('{{message}}', msg).replace('{{owner-class}}', player ? 'player' : 'ai');
    templateContainer.innerHTML += newMessage;
};

const ShowInputAction = (show) => {
    document.getElementById('unity-chat-input').style.display = show ? 'block' : 'none';
}

ShowInputAction(false);