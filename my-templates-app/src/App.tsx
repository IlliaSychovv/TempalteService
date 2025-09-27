import { useState } from 'react';
import type {Template} from './types';
import { TemplateList } from './components/TemplateList';
import { CreateTemplateForm } from './components/CreateTemplateForm';
import { EditTemplateForm } from './components/EditTemplateForm';
import { GeneratePdfForm } from './components/GeneratePdfForm';
import './App.css';

type ViewMode = 'list' | 'create' | 'edit' | 'generate';

function App() {
  const [currentView, setCurrentView] = useState<ViewMode>('list');
  const [selectedTemplate, setSelectedTemplate] = useState<Template | null>(null);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handleCreateNew = () => {
    setCurrentView('create');
  };

  const handleEdit = (template: Template) => {
    setSelectedTemplate(template);
    setCurrentView('edit');
  };

  const handleGeneratePdf = (template: Template) => {
    setSelectedTemplate(template);
    setCurrentView('generate');
  };

  const handleBackToList = () => {
    setCurrentView('list');
    setSelectedTemplate(null);
    setRefreshTrigger(prev => prev + 1);
  };

  const renderCurrentView = () => {
    switch (currentView) {
      case 'create':
        return <CreateTemplateForm onSuccess={handleBackToList} onCancel={handleBackToList} />;
      
      case 'edit':
        return selectedTemplate ? (
          <EditTemplateForm 
            template={selectedTemplate} 
            onSuccess={handleBackToList} 
            onCancel={handleBackToList} 
          />
        ) : null;
      
      case 'generate':
        return selectedTemplate ? (
          <GeneratePdfForm 
            template={selectedTemplate} 
            onSuccess={handleBackToList} 
            onCancel={handleBackToList} 
          />
        ) : null;
      
      default:
        return (
          <>
            <div className="app-header">
              <h1>Template management system</h1>
              <button onClick={handleCreateNew} className="btn btn-primary">
                Create a new template
              </button>
            </div>
            <TemplateList 
              onEdit={handleEdit}
              onGeneratePdf={handleGeneratePdf}
              refreshTrigger={refreshTrigger}
            />
          </>
        );
    }
  };

  return (
    <div className="app">
      {renderCurrentView()}
    </div>
  );
}

export default App;
