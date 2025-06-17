# Fake Address API
The Fake Address API utilizes the [Bogus](https://www.nuget.org/packages/Bogus) package to generate addresses for testing purposes.  Everything is stored in memory so there is no static data except one [default address](#default-address).

## API Endpoints
- GET `/addresses` - returns all addresses
- GET `/addresses?state=<state>` - returns addresses in a specific state
- GET `/addresses?zip=<zip>` - returns addresses in a specific zip code
- POST `/new-addresses` - clears current addresses and creates new addresses based on the body `count` input property

  ```json
  {
    "count": 1000
  }
  ```
- POST `/append-addresses` - appends new addresses based on the body `count` input property

  ```json
  {
    "count": 100
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