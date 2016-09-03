Welcome to NWheels
=======

According to our observations, commonality in the needs of enterprise application projects is significantly higher than variability. We take this as an opportunity to slash project costs and timeframes, improve software quality, and reduce technical risks. 

(see [Motivation and goals](https://github.com/felix-b/NWheels/wiki/Motivation-and-goals) in Wiki)

## What is NWheels

NWheels is an infrastructural ecosystem and ongoing development effort, which is aimed to supply A-to-Z architectural recipes, innovative runtime platform, and high productivity development framework for enterprise applications, based on .NET Core platform. 

(see [Highlights](#highlights) below)

NWheels is released under the **MIT license**, and is aimed to stay **free forever**.

#### NWheels is Recipe

NWheels implements architectural recipes that address the whole story of a typical enterprise application. Among various aspects, the architecture covers: data persistence and caching, several architectural flavors of business logic organization, user interface on multiple platforms/devices, B2B integrations, testability, extensibility and customization, scalability and resilience, and DevOps/ALM toolchain. 

#### NWheels is Interface

Rather than being a monolith that attempts to solve every problem, NWheels only solves one problem, and it does it well. NWheels defines a consistent API, and supplies a set of core services to pluggable modules. It is then pluggable modules that define application capabilities, productivity frameworks for application development, and technology choices.

#### NWheels is Generator

The amount of code that an application developer has to write is reduced down to declaration and implementation of unique features. Though great deal of reduction is gained from reuse of building block domains and technology stack adapters, there are still concrete application models, which have to be connected with generic infrastructure mechanisms. 

The latter is traditionally accomplished by layers of repetitive and mechanical code, considered integral part of application models. NWheels removes the burden of coding those layers, by generating code through pipelines of pluggable conventions.

#### NWheels is Home

NWheels is aimed to become a place where collective expertise of enterprise application development is accumulated. Collaboration and expertise sharing is possible though development of reusable and adaptive NWheels modules, and by contribution to the surrounding ecosystem of developer resources.

#### NWheels is Open Source Software

We believe that source sharing and collaboration, driven by enthusiasm for quality and for professionalism, have much better chances of delivering working and (re)usable software, rather than isolated teams driven by sales plans of profit-oriented organizations. 

## Current Status

NWheels has not yet delivered its platform for general availability. By now, a proof-of-concept version was incubated, named **milestone Afra**. It is stable and serves a basis for two proprietary real-world applications. 

#### September 2016

- **Milestone Afra - completed** - a proof-of-concept version was developed, and two proprietary real-world applications were built on top of it. This allowed validation of architectural concepts. This version of the platform was mostly one-person project, with 2 more developers involved in development of the applications. 

#### Next Step 

- **Milestone Boda** - starting with **fresh new codebase** and switching to **community-driven phase**. It will be based on the lessons learned from milestone Afra, on well documented architecture and feature designs, and on contributions from community. 

## Highlights

NWheels is aimed to exhibit the following characteristics:

#### A-to-Z response to common demand
  - one framework that covers all application layers and tiers: projects are not left to sweat over gluing multiple 3rd-party building blocks together
    - that is not to say a monolith, which tries to solve every problem: NWheels is a consistent interface for pluggable modules, where each module does best at its area of responsibility. 
  - ready answers to common requirements and concerns, ranging from basic features like authorization, to advanced scenarios like elastic scalability
  - built-in support for DevOps procedures, automation of clouds, and easy integration with application lifecycle 
management

#### Get significantly more for doing much less

  - scaffold a new application - and have it automatically built, deployed, and monitored on cloud or on premises environments, where the only piece that is missing, is the unique features you are going to develop.
  - code domain model, logic, and conceptual UI - and get whole layers such as UI apps, data persistence, and REST/backend APIs, automatically implemented by conventions.
  - use Information Security building block domain - and get user account management, authentication, and common user stories such as  'confirm email' and 'change password', out of the box.
  - define access control rules for different user profiles - and have them transparently enforced through all application layers, including access to both operations and data.
  - define semantic logging messages - and get automatic metric collection, thresholds, circuit breakers, and alerts.

#### Proven architectures, approaches, and patterns, for dramatically less effort on your side
  - micro-services
  - hexagonal architecture 
  - domain-driven design 

#### Innovative approaches
  - convention over implementation - transparent implementation of abstractions by pipelines of pluggable conventions - an approach, which eliminates majority of repetitive mechanical code from your codebase.
  - layered unobtrusive customization - multiple reusable orthogonal adaptations are stacked on top of white-label version. Plugged into customer-specific configurations, the adaptations extend and alter domain model, logic, and conceptual UI, while the white-label version remains unchanged. 
  - late compilation - model-based components are late-compiled against customized models and concrete technology stacks
  - building block domains - adaptive and reusable models, logic, and conceptual UI parts for common domains, such as e-commerce, CRM, booking, accounting.

#### Platform at your service
  - communication endpoints, backend APIs, messaging, workflows, scheduled jobs, and more
  - elastic on-demand scalability and failover redundancy
  - cloud, hybrid, and on-premise deployments
  - no need to depend on cloud vendor PaaS - no vendor lock-in

#### Ready DevOps/ALM toolchain
  - automated deployment to dev boxes and test/prod environments on premise, hybrid, and on cloud
  - runtime health monitoring, metric collection, and tools for production intelligence
  - continuous deployment and continuous integration with optional developer git flow, personal builds, and gated commits
  - product and agile process management
  - all of the above is cross-tracked for maximal visibility and decision support

### What types of applications can be built

NWheels is primarily aimed to support enterprise applications, of any size and complexity.

#### Architectures

- Typical N-tier applications, which consist of:
  - application tier, composed of one or more micro-services that execute business logic
  - communication endpoints, exposed by application tier for client UI apps and B2B integrations
  - data tier, containing any number of databases, possibly of different vendors and technologies
  - UI apps on various presentation platforms, such as web browser (single-page app), desktop GUI, native mobile apps, and less common ones like Smart TV and IVR. 
- Serverless architecture is naturally achieved by:
  - giving up explicit service boundaries
  - letting domain models and logic be transparently hosted and scaled by the platform

#### SLA categories
  
- non-business-critical
- business-critical (9x5) and mission-critical (24x7)
- low-latency and high-throughput processing, e.g. trading
  
### Where the applications can run 

- Server side: the platform targets .NET Core, thus server-side components can run on Windows, Linux, or Mac servers.
- Client side: can run on variety of presentation platforms, as mentioned earlier.
 
### Read more

- Project Wiki - comprehensive information for both consumers and contributors
- Introduction
- Motivation and goals
- Feature explorer

## Milestones

#### Milestone Afra - completed

[![Build status](https://ci.appveyor.com/api/projects/status/x0xcs9lfg4tee88s?svg=true)]
(https://ci.appveyor.com/project/felix-b/nwheels)

The first milestone was fundamental in that it included development of core components, and validation of architectural concept, through building several real-world applications on top of it.

This milestone was mostly one-person project, with no community involved. 

The following was done:
- minimal set of core features was developed, enough for a simple typical enterprise application 
- two real-world applications were built on top of the platform
- architectural concept and feasibility of implementation were proven
- a lot of lessons learned

#### Milestone Boda - starting

This milestone starts with a fresh new codebase. 

Targets:
- document overall architecture and details of the planned features
- start building community of contributors
- work with the community to refine architecture and feature designs
- proceed with development of the platform, targeting .NET Core

The decision to start a new codebase was for these reasons:
- take full benefits of lessons learned in Milestone Afra
- write code clean from numerous deficiencies and technical debts found in Milestone Afra
- use a more elegant and friendly library for implementation-by-convention and late-compilation 
- target .NET Core
- let the community build knowledge and take ownership of the entire codebase

