# FAQ

1. Why throw exception at service layer and not other layer?
- Separation of Concerns.
- Easy to control errors in a single place.
- Controller only needs to handle responses, no need to worry about business.
- Simple repository, easy to reuse.
- Easy to maintain, extend system with middleware and logging.