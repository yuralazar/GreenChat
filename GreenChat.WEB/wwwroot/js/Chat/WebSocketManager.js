
var Connection = (function () {

    function Connection(url, enableLogging)
    {
        var _this = this;
        if (enableLogging === void 0) { enableLogging = false; }
        this.enableLogging = false;
        this.clientMethods = {};
        this.connectionMethods = {};
        this.url = url;
        this.enableLogging = enableLogging;
        this.connectionMethods['onConnected'] = function () {
            if (_this.enableLogging) {
                console.log('Connected! socketId: ' + _this.socketId);
            }
        };
        this.connectionMethods['onDisconnected'] = function () {
            if (_this.enableLogging) {
                console.log('Connection closed from: ' + _this.url);
            }
        };
        this.connectionMethods['onOpen'] = function (socketOpenedEvent) {
            if (_this.enableLogging) {
                console.log("WebSockets connection opened (" + new Date() + ")");
            }
        };
        this.connectionMethods['onSelfClose'] = function () {
            if (_this.enableLogging) {
                console.log("WebSockets connection closed (" + new Date() + ")");
            }
        };
    }

    Connection.prototype.start = function () {
        var _this = this;
        this.socket = new WebSocket(this.url);                      
        this.socket.onopen = function (event) {
            _this.connectionMethods['onOpen'].apply(_this, event);
        };
        this.socket.onmessage = function (event) {
            
            _this.message = JSON.parse(event.data);
            if (_this.enableLogging)
                console.log(_this.message);                    
            var numberType = _this.message.type;
            var messageArgs = JSON.parse(_this.message.arguments);
            var methodName = "";

            switch (numberType) {
                case 0:
                    if (_this.enableLogging) console.log('Server Error :' + _this.message.arguments);
                    break;
                case 1: 
                    if (messageArgs.online)
                        _this.connectionMethods['onConnected'].apply(_this, [{ "user": messageArgs }]);
                    else
                        _this.connectionMethods['onDisconnected'].apply(_this, [{ "user": messageArgs }]);
                    break;
                case 2: methodName = "PrivateMessage"; break;
                case 3: methodName = "ChatMessage"; break;                    
                case 4: methodName = "UserFound"; break;
                case 5: methodName = "FriendRequest"; break;
                case 6: methodName = "FriendConfirmed"; break;
                case 7: methodName = "ChatCreated"; break;
                case 8: methodName = "ChatRequest"; break;
                case 9: methodName = "ChatConfirmed"; break;
                case 10: methodName = "InitialInfo"; break;
                default:
            };

            if(methodName !== "")
                _this.clientMethods[methodName].apply(_this, [{ "data": messageArgs }]);
        };

        this.socket.onclose = function (event) {                            
            _this.connectionMethods['onSelfClose'].apply(_this);
        };
        this.socket.onerror = function (event) {
            if (_this.enableLogging) {
                console.log('Error data: ' + event.error);
            }
        };
    };

    Connection.prototype.invoke = function (obj) {                        
        if (this.enableLogging) {
            console.log(obj);
        }
        var jsonData = JSON.stringify(obj);
        console.log("Sending json - " + jsonData);
        this.socket.send(jsonData);
    };

    return Connection;

}());