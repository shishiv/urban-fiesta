# FormNotificacoes Responsive Layout - Meta-Prompt Pipeline

## Overview
This pipeline transforms FormNotificacoes from fixed-position layout to responsive layout using a 3-stage Claude-to-Claude workflow.

## Pipeline Stages

### Stage 1: Research (`01-formnotificacoes-responsive-research.md`)
**Purpose**: Analyze current implementation and document required changes
**Output**: Structured XML with component inventory, hardcoded values, and recommendations

### Stage 2: Plan (`02-formnotificacoes-responsive-plan.md`)
**Purpose**: Create detailed implementation plan from research findings
**Output**: Ordered steps with exact code changes and verification criteria

### Stage 3: Implement (`03-formnotificacoes-responsive-implement.md`)
**Purpose**: Execute the implementation plan
**Output**: Modified FormNotificacoes.cs with responsive layout

## Execution

### Option A: Sequential (Recommended for complex changes)
```
/taches-cc-resources:run-prompt 01 --sequential
/taches-cc-resources:run-prompt 02 --sequential
/taches-cc-resources:run-prompt 03 --sequential
```

### Option B: Direct Implementation
If you want to skip research/planning and implement directly:
```
/taches-cc-resources:run-prompt 03
```

## Requirements Summary

| Requirement | Value |
|------------|-------|
| MinimumSize | 800x500 |
| AutoScroll | true |
| Header | Dock.Top with title Anchor Top\|Left\|Right |
| DataGridView | Dock.Fill (was Anchor Top\|Bottom\|Left\|Right) |
| Action Buttons | FlowLayoutPanel, Dock.Bottom |
| Close Button | Anchor Bottom\|Right |

## Preserved Functionality
- Filter system (Status, Type, Priority)
- Mark as read/unread operations
- Delete notification with confirmation
- Auto-refresh timer (30 seconds)
- Priority-based row coloring (ALTA, MEDIA, BAIXA)
- NotificacaoDAL integration
- Selection-based button enabling

## Validation
Build must succeed:
```bash
cd 06_bibliotecaJK && dotnet build BibliotecaJK.csproj
```
