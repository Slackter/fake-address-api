# Fake Address API
The Fake Address API is a simple quick way to generate addresses for testing purposes.  Everything is stored in memory so there is no static data except one [Default address](#default-address)

## API Endpoints
- GET `/addresses` - returns all addresses
- GET `/addresses?state=<state>` - returns addresses in a specific state
- GET `/addresses?zip=<zip>` - returns addresses in a specific zip code
- POST `/new-addresses` - wipes out current addresses and creates new addresses

  ```json
  {
    "count": <address count>
  }
  ```
- POST `/append-addresses` - appends new addresses

  ```json
  {
    "count": <address count>
  }
  ```

## Default address
By default, this address is always there for testing purposes
```json
{
  "street": "1 Hardcoded Way",
  "city": "The Woods",
  "state": "RI",
  "zipCode": "02814"
}
```