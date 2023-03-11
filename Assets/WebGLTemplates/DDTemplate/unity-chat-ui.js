// Define the template using a template literal
const templateMessage = `
	<h1>Hello, {{name}}!</h1>
	<p>You are {{age}} years old.</p>
`;

// Rewrite Test() to use modern syntax
const Test = (args) => {
	console.log('Hello, World!');
	console.log('args:', args);
	
	// Add the instantiated template to the DOM
	const templateContainer = document.getElementById('unity-chat');
	// append to inner contents instead
	templateContainer.innerHTML += templateMessage.replace('{{name}}', 'John').replace('{{age}}', 30);
};