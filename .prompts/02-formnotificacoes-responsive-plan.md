# Plan Prompt: FormNotificacoes Responsive Layout Implementation

## Objective
Create a detailed implementation plan for making FormNotificacoes responsive, using research findings from the previous stage.

## Input
Receives `<research-findings>` from Stage 01 containing:
- Component inventory with current positions
- Hardcoded values to replace
- Logic to preserve
- Responsive recommendations

## Planning Requirements

### 1. Define Responsive Layout Architecture

```
Form (MinimumSize: 800x500, AutoScroll: true)
├── pnlHeader (Dock: Top, Height: 80)
│   ├── lblTitulo (Anchor: Top|Left|Right)
│   ├── lblNaoLidas (Anchor: Top|Left)
│   └── lblTotal (Anchor: Top|Left)
├── pnlFiltros (Dock: Top, Height: 60)
│   ├── Filter controls (Anchor: Top|Left)
│   └── btnAtualizar (Anchor: Top|Right)
├── dgvNotificacoes (Dock: Fill)
└── pnlAcoes (Dock: Bottom, Height: 60)
    ├── Action buttons (FlowLayoutPanel, Anchor: Left)
    └── btnFechar (Anchor: Right)
```

### 2. Specify Exact Property Changes

For each control, specify:
- Dock property (if applicable)
- Anchor property (if applicable)
- MinimumSize (if needed)
- AutoSize settings
- Padding/Margin adjustments

### 3. Define Implementation Order

Order changes to minimize conflicts:
1. Form-level properties first
2. Container panels (outer to inner)
3. DataGridView
4. Individual controls within panels

### 4. Identify Code Changes

List exact code modifications:
- Lines to modify
- Properties to add
- Properties to remove
- New controls needed (FlowLayoutPanel)

## Output Requirements

Produce implementation plan in this format:

```xml
<implementation-plan>
  <overview>
    <approach>[Brief description of responsive strategy]</approach>
    <risk-level>LOW|MEDIUM|HIGH</risk-level>
    <estimated-changes>[number] lines modified</estimated-changes>
  </overview>

  <step sequence="1">
    <target>Form Properties</target>
    <action>Add MinimumSize and AutoScroll</action>
    <code-changes>
      <add-after line="49">
this.MinimumSize = new Size(800, 500);
this.AutoScroll = true;
      </add-after>
    </code-changes>
    <verification>Form should not resize below 800x500</verification>
  </step>

  <step sequence="2">
    <target>pnlHeader</target>
    <action>Change from fixed position to Dock.Top</action>
    <code-changes>
      <remove>Location = new Point(0, 0),</remove>
      <remove>Size = new Size(1100, 80),</remove>
      <add>Dock = DockStyle.Top,</add>
      <add>Height = 80,</add>
    </code-changes>
    <verification>Header should stretch full width on resize</verification>
  </step>

  <!-- Continue for all components -->

  <preserved-functionality>
    <item>Filter dropdowns trigger CarregarNotificacoes()</item>
    <item>BtnMarcarLida_Click marks selected notification as read</item>
    <item>BtnMarcarTodasLidas_Click marks all as read with confirmation</item>
    <item>BtnExcluir_Click deletes with confirmation</item>
    <item>30-second auto-refresh timer</item>
    <item>Priority-based row coloring in DgvNotificacoes_CellFormatting</item>
    <item>Selection-based button enabling in DgvNotificacoes_SelectionChanged</item>
  </preserved-functionality>

  <testing-checklist>
    <test>Resize form to minimum size (800x500)</test>
    <test>Resize form to maximum size</test>
    <test>Verify all filters work correctly</test>
    <test>Verify mark as read functionality</test>
    <test>Verify delete functionality</test>
    <test>Verify priority colors display correctly</test>
    <test>Build without errors or warnings</test>
  </testing-checklist>
</implementation-plan>
```

## Constraints
- MUST preserve all existing functionality
- MUST NOT change color scheme or visual style
- MUST NOT modify business logic methods
- MUST use only standard WinForms anchoring/docking

## Success Criteria
- [ ] Every UI component has defined responsive behavior
- [ ] Implementation steps are sequentially ordered
- [ ] Code changes are explicit and copy-pasteable
- [ ] Verification steps defined for each change
- [ ] Testing checklist covers all preserved functionality
