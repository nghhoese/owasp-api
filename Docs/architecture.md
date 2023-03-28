# Architecture Diagram

```mermaid
graph TD
    B(API) --- C{Business Logic}
    C --- F{Entities}
    C --- D{Data Layer}
    D --- F{Entities}
    D --- E[(Database)]
```