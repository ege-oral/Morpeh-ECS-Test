# Code Style Guide

## Context

Global code style rules for Unity C# projects in Agent OS.

<conditional-block context-check="general-formatting">
IF this General Formatting section already read in current context:
  SKIP: Re-reading this section
  NOTE: "Using General Formatting rules already in context"
ELSE:
  READ: The following formatting rules

## General Formatting

### Indentation
- Use 4 spaces for indentation (Unity/C# standard)
- Maintain consistent indentation throughout files
- Align nested structures for readability

### Naming Conventions
- **Methods and Properties**: Use PascalCase (e.g., `GetComponent`, `OnUpdate`)
- **Variables and Fields**: Use camelCase (e.g., `entityCount`, `deltaTime`)
- **Private Fields**: Use underscore prefix (e.g., `_filter`, `_entityViewManager`)
- **Classes and Structs**: Use PascalCase (e.g., `PlayerMovementSystem`, `HealthComponent`)
- **Constants**: Use PascalCase (e.g., `MaxHealth`, `SpawnRate`)
- **Namespaces**: Use PascalCase with dots (e.g., `ECS.Systems.Player`)

### String Formatting
- Use double quotes for strings: `"Hello World"`
- Use string interpolation for dynamic content: `$"Health: {health}"`
- Use verbatim strings for paths: `@"Assets\Prefabs\Player"`

### Code Comments
- Use XML documentation for public APIs: `/// <summary>`
- Add brief comments above non-obvious ECS logic
- Document component purposes and system responsibilities
- Explain performance optimizations (IL2CPP attributes, etc.)
- Never remove existing comments unless removing the associated code
- Update comments when modifying code to maintain accuracy
- Keep comments concise and relevant

### C# Specific Rules
- Use `var` when type is obvious: `var entity = World.CreateEntity();`
- Use explicit types for clarity: `Entity playerEntity = ...;`
- Prefer readonly fields where possible
- Use ref returns for component access: `ref var component = ref stash.Get(entity);`
- Implement IDisposable for systems and managers
</conditional-block>

## ECS Architecture Guidelines

### Component Design
- Components should be pure data containers (structs preferred)
- Avoid logic in components - keep them simple
- Use meaningful names that describe the data's purpose
- Mark components as readonly when possible
- Implement IComponent interface

### System Design
- Systems contain logic, never data
- Systems should be stateless where possible
- Use dependency injection for external dependencies
- Implement proper disposal in systems
- Use IL2CPP optimization attributes for performance-critical systems
- Filter entities efficiently using World.Filter

### Performance Patterns
- Cache component stashes in OnAwake()
- Use ref returns to avoid copying structs
- Prefer foreach over other iteration methods
- Mark hot paths with [MethodImpl(MethodImplOptions.AggressiveInlining)]
- Use burst-compatible code where possible

### Entity Management
- Use object pooling for frequently created/destroyed entities
- Tag entities with meaningful components for filtering
- Clean up entity references properly in Dispose methods
- Avoid storing entity references long-term
