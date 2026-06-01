This project is the Web Programming Course Final Project for the 2025-2026 Fall Semester.

```markdown
# Fitness Center Management and Appointment System

A comprehensive, enterprise-grade web application developed as the **Web Programming Course Final Project** for the **2025–2026 Fall Semester**. This project is built using **ASP.NET Core MVC** and demonstrates the application of modern full-stack web development principles, database communications, and advanced features integrated into a real-world software system designed for sports facilities and gym management.

## Key Project Features

### 1. Service and Coach Management
* **Dynamic Gym Services:** Full administrative controls (CRUD operations) for managing diverse fitness services, gym memberships, and training packages.
* **Trainer Profile & Specialty Management:** Comprehensive modules to manage coaches, link them to specific fitness specialties (e.g., bodybuilding, yoga, crossfit), and track their operational shifts.

### 2. Member Appointment System
* **Real-time Scheduling:** An automated booking engine allowing registered gym members to view real-time availability of coaches and services.
* **Conflict Prevention:** Smart validation system ensuring coaches are never double-booked and members can seamlessly cancel or reschedule within appropriate time windows.

### 3. Artificial Intelligence (AI) Integration
* **Personalized Wellness Recommendations:** Features an embedded AI engine that processes custom user attributes to generate tailor-made exercise regimes and meal plans.
* **Multimodal Input Processing:** Processes body metrics (weight, body type, target goals) alongside user-uploaded physical photographs to dynamically adjust recommendation models.

### 4. Database Communication & Reporting (REST API)
* **API-Driven Communications:** The application incorporates a dedicated REST API architecture to handle internal database transactions, data exchanges, and service actions securely.
* **Reporting & Analytics Dashboard:** Utilizes RESTful endpoints to pull real-time operational data, generating business reports on peak operational hours, trainer utilization rates, and popular services.


##  Technology Stack & Architecture

* **Frontend & UI Layer:** Razor Views (HTML5, CSS3, JavaScript), Bootstrap 5, and jQuery for dynamic UI manipulation.
* **Application Framework:** ASP.NET Core MVC (Model-View-Controller) leveraging dependency injection, strongly-typed views, and clean separation of concerns.
* **Backend & API Layer:** Built-in REST API endpoints within the ecosystem to ensure robust JSON data delivery and reporting capabilities.
* **Database & ORM:** Microsoft SQL Server integrated via **Entity Framework Core (EF Core)** utilizing Code-First migrations.
* **AI Integration Layer:** Connects to cloud-based AI REST endpoints (e.g., OpenAI or custom API services) to evaluate user inputs and return smart lifestyle data.


##  Project Architecture Overview

Based on the solution layout, the project is structured under a unified single-project MVC pattern, organizing core components cleanly within standard directory lifecycles:

```text
└── WebProgramlamaFinalProject/
    ├── Controllers/     # Handles HTTP requests, MVC routing, and REST API endpoints
    ├── Models/          # Contains database entities, view models, and domain logic
    ├── Views/           # Razor-based frontend templates (.cshtml pages)
    ├── Migrations/      # Entity Framework Core database schema history
    └── wwwroot/         # Static assets (CSS, JavaScript, Images, Bootstrap)
