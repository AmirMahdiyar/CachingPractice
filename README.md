# Distributed Hybrid Caching with .NET 9, Redis & RabbitMQ

This project implements a sophisticated **L1/L2 Caching Strategy** using the new `Microsoft.Extensions.Caching.Hybrid` library. It specifically addresses the challenge of **Cache Coherency** in distributed environments by using **RabbitMQ** as a signaling backplane.



---

## 🏗 System Architecture

The goal of this architecture is to minimize latency while ensuring data consistency across multiple application instances.

1.  **L1 (Local Memory Cache):** The fastest tier. Data is stored directly in the application's RAM.
2.  **L2 (Distributed Redis):** Shared among all instances. If L1 misses, the app fetches from Redis to avoid hitting the database.
3.  **Backplane (RabbitMQ via MassTransit):** When an update occurs on one node, a message is broadcasted to all other nodes to invalidate their local L1 caches, preventing stale data.
4.  **Persistent Store (SQL Server):** The source of truth accessed via **EF Core**.

---

## 🛠 Tech Stack

* **Runtime:** .NET 9 (Preview/RC)
* **Caching:** `Microsoft.Extensions.Caching.Hybrid`
* **Distributed Cache:** Redis
* **Messaging:** MassTransit + RabbitMQ
* **Database:** SQL Server + Entity Framework Core

---

## 🔥 Key Implementation Details

### 1. HybridCache Integration
Unlike the traditional `IDistributedCache`, the `HybridCache` handles **Cache Stampede** (thundering herd) protection by ensuring that for a specific key, only one request actually hits the factory (database) while others wait for the result.

### 2. The RabbitMQ Invalidation Loop
Because L1 cache is local to each instance, updating a record on **Server A** leaves **Server B** with "stale" data in its memory. 

* **Step 1:** Data is updated via EF Core.
* **Step 2:** A `CacheInvalidationMessage` is published to a RabbitMQ **Fanout Exchange**.
* **Step 3:** Every running instance has its own private queue bound to that exchange.
* **Step 4:** The MassTransit Consumer receives the message and calls `_hybridCache.RemoveAsync(key)`, clearing the local L1.

### 3. Hosted Service for Syncing (Background Processing)
To keep the API highly responsive, we implemented a dedicated background synchronization layer:
* **Producer-Consumer Pattern:** The Controller pushes heavy tasks (like cache re-population or post-delete syncs) into a thread-safe `System.Threading.Channels.Channel`.
* **The Worker:** A `BackgroundService` (Worker) dequeues these tasks and executes them asynchronously.
* **Service Scoping:** Since the worker is a Singleton, we used `IServiceScopeFactory` to create a new scope for each task. This allows the worker to safely use **Scoped** dependencies like `DbContext` without encountering `ObjectDisposedException`.

### 4. Fluent Builder Pattern
The startup configuration is refactored into a **Fluent API** style using Extension Methods. This keeps the `Program.cs` clean and modular:
```csharp
var builder = WebApplication.CreateBuilder(args)
    .AddCaching()
    .AddDatabase()
    .AddMessageBroker()
    .AddHostedServices(); // Includes our Channel Workers
---



