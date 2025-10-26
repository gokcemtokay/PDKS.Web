#!/bin/bash
cd /home/claude/pdks-frontend-full

# src/main.tsx
cat > src/main.tsx << 'TSXEOF'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
TSXEOF

# src/index.css
cat > src/index.css << 'CSSEOF'
:root {
  font-family: 'Roboto', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  line-height: 1.5;
  font-weight: 400;
  color-scheme: light dark;
  font-synthesis: none;
  text-rendering: optimizeLegibility;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  margin: 0;
  min-height: 100vh;
}

#root {
  min-height: 100vh;
}
CSSEOF

# src/vite-env.d.ts
cat > src/vite-env.d.ts << 'TSEOF'
/// <reference types="vite/client" />
TSEOF

echo "Core dosyalar oluşturuldu!"
