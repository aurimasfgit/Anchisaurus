# Anchisaurus

WEB API where user can store key-value pairs. Client should be authorized (API KEY should be issued to client) to call endpoints.

API-KEYs for testing purposes: 
- 123456789
- 55555
- 123

For simplicity (don't need to install anything), LiteDB (embedded NoSQL database) is prepared to be used with this project.

# To do

- In "ApiKeyService" API keys and clients are hard-coded for now. Need to load them from persistent storage (DB, etc.)
- Implement "DbKeyValuePairWithExpirationService" methods that will allow to store data (key-value pairs) in database
- Refactor Web API exception handling (different exception type catching, logging, returning different responses to client)
- Add logging to persistent storage (file, Elasticsearch, etc.)
