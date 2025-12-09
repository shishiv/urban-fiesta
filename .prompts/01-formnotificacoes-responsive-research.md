# Research Prompt: FormNotificacoes Responsive Layout Analysis

## Objective
Analyze the current FormNotificacoes implementation and document the exact changes needed to implement responsive layout while preserving all existing functionality.

## Context
FormNotificacoes is a Windows Forms notification center in a C# .NET 8.0 library management system. The current implementation uses fixed positioning (Location property) without anchoring, making it non-responsive to window resizing.

## Current State Analysis Required

### 1. Identify All UI Components
Document each control with its current positioning:
- Header Panel (pnlHeader)
- Filter Panel (pnlFiltros) with ComboBoxes and Labels
- DataGridView (dgvNotificacoes)
- Action Panel (pnlAcoes) with Buttons

### 2. Map Current Fixed Values
Extract all hardcoded dimensions:
- Form ClientSize: 1100x700
- Panel locations and sizes
- Button locations and sizes
- DataGridView location and size
- All control spacing values

### 3. Document Business Logic to Preserve
List all functionality that MUST remain unchanged:
- Filter system (Status, Type, Priority)
- Mark as read/unread operations
- Delete notification
- Auto-refresh timer (30 seconds)
- Priority-based row coloring
- Selection-based button enabling

### 4. Identify Integration Points
- NotificacaoDAL dependency
- TipoNotificacao enum values
- PrioridadeNotificacao enum values

## Output Requirements

Produce a structured analysis in this format:

```xml
<research-findings>
  <component-inventory>
    <component name="pnlHeader">
      <current-position>Location(0,0), Size(1100,80)</current-position>
      <contains>lblTitulo, lblNaoLidas, lblTotal</contains>
      <responsive-strategy>[recommended approach]</responsive-strategy>
    </component>
    <!-- repeat for all components -->
  </component-inventory>

  <hardcoded-values>
    <value name="FormSize" current="1100x700" min-recommended="800x500"/>
    <!-- list all values -->
  </hardcoded-values>

  <preserved-logic>
    <logic name="FilterSystem">
      <description>...</description>
      <methods-involved>CarregarNotificacoes</methods-involved>
    </logic>
    <!-- list all logic -->
  </preserved-logic>

  <responsive-recommendations>
    <recommendation priority="1">
      <target>Form</target>
      <change>Set MinimumSize = new Size(800, 500)</change>
    </recommendation>
    <!-- ordered list of changes -->
  </responsive-recommendations>
</research-findings>
```

## Constraints
- Do NOT modify any business logic
- Do NOT change the visual design/colors
- Do NOT alter DAL integration patterns
- Focus ONLY on layout responsiveness

## Success Criteria
- [ ] All UI components documented with current positions
- [ ] All hardcoded values identified
- [ ] Clear responsive strategy for each component
- [ ] Preserved logic explicitly listed
- [ ] Recommendations ordered by implementation priority
