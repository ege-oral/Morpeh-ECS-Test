# Development Best Practices

## Context

Global development guidelines for Unity ECS projects in Agent OS.

<conditional-block context-check="core-principles">
IF this Core Principles section already read in current context:
  SKIP: Re-reading this section
  NOTE: "Using Core Principles already in context"
ELSE:
  READ: The following principles

## Core Principles

### Keep It Simple
- Implement ECS systems with single responsibilities
- Avoid complex entity hierarchies
- Choose data-oriented design over object-oriented patterns

### Optimize for Performance
- Design for cache-friendly data access patterns
- Use component pooling for frequently created entities
- Profile before optimizing, measure performance impact
- Leverage IL2CPP and Burst compilation for hot paths

### ECS Architecture Principles
- Components are data-only (no behavior)
- Systems contain all logic and behavior
- Entities are just IDs - avoid storing references
- Use composition over inheritance

### File Structure
- Organize by feature areas (Player, Enemy, Combat)
- Separate Components, Systems, and Providers into folders
- Use consistent naming conventions for ECS elements
- Group related systems and components together
</conditional-block>

## ECS Specific Practices

### Entity Lifecycle Management
- Create entities through providers or factories
- Use pooling for entities that are frequently created/destroyed
- Clean up entity references in system Dispose methods
- Avoid storing entity references across frames

### Component Design Patterns
- Keep components as small as possible (avoid "god components")
- Use tag components for entity categorization
- Prefer value types (structs) over reference types
- Make components immutable when possible

### System Implementation
- Initialize filters and stashes in OnAwake()
- Process entities in batch operations when possible
- Use early returns to skip unnecessary processing
- Implement proper resource cleanup in Dispose()

### Performance Optimization
- Cache component stashes to avoid repeated lookups
- Use ref returns to avoid struct copying
- Apply IL2CPP attributes to hot path systems
- Profile with Unity's Profiler and optimize bottlenecks
- Consider job system integration for parallel processing

### Testing Strategies
- Unit test system logic in isolation
- Test component data transformations
- Use Unity Test Framework for integration tests
- Mock external dependencies (like EntityViewManager)
