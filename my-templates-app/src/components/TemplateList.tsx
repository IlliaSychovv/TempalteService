import { useState, useEffect } from 'react';
import type {Template} from '../types';
import { TemplaterApi } from '../api/templaterApi';
import './TemplateList.css';

interface TemplateListProps {
  onEdit: (template: Template) => void;
  onGeneratePdf: (template: Template) => void;
  refreshTrigger: number;
}

export function TemplateList({ onEdit, onGeneratePdf, refreshTrigger }: TemplateListProps) {
  const [templates, setTemplates] = useState<Template[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadTemplates();
  }, [refreshTrigger]);

  const loadTemplates = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await TemplaterApi.getTemplates();
      setTemplates(response.item);
    } catch (err) {
      setError('Error loading templates');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this template?')) {
      return;
    }

    try {
      await TemplaterApi.deleteTemplate(id);
      await loadTemplates();
    } catch (err) {
      alert('Template deletion error');
      console.error(err);
    }
  };

  if (loading) {
    return <div className="loading">Downloading templates...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  if (templates.length === 0) {
    return <div className="empty">No templates found</div>;
  }

  return (
    <div className="template-list">
      <h2>List of templates</h2>
      <div className="templates-grid">
        {templates.map((template) => (
          <div key={template.id} className="template-card">
            <h3>{template.name}</h3>
            <div className="template-meta">
              <small>Created: {new Date(template.createdAt).toLocaleDateString()}</small>
              {template.modifiedAt && (
                <small>Changed: {new Date(template.modifiedAt).toLocaleDateString()}</small>
              )}
            </div>
            <div className="template-content">
              <pre>{template.htmlContent.substring(0, 100)}...</pre>
            </div>
            <div className="template-actions">
              <button onClick={() => onEdit(template)} className="btn btn-edit">
                Edit
              </button>
              <button onClick={() => onGeneratePdf(template)} className="btn btn-generate">
                Generate PDF
              </button>
              <button onClick={() => handleDelete(template.id)} className="btn btn-delete">
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
