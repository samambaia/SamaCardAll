function timedAlert(message, duration) {
    // Create alert element 
    const alertDiv = document.createElement('div');
    alertDiv.textContent = message;
    alertDiv.style.border = "2px solid #1861ac"; // Add a border
    alertDiv.style.position = 'fixed';
    alertDiv.style.padding = 28;
    alertDiv.style.top = '7%'; // Position at top
    alertDiv.style.right = '10%'; // Position at left
    //alertDiv.style.backgroundColor = '#1b6ec2'
    alertDiv.style.boxShadow = '2px 2px 5px rgba(0, 0, 0, 0.2)';
    alertDiv.style.backgroundColor = 'rgba(255, 255, 255, 0.9)'; // 0.9 for 90% opacity

    document.body.appendChild(alertDiv);

    // Auto-close after duration 
    setTimeout(() => {
        document.body.removeChild(alertDiv);
    }, duration);
}
