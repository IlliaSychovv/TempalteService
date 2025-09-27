import type {Template, CreateTemplateDto, UpdateTemplateDto, GeneratePdfDto, PagedResponse} from '../types';

const API_BASE_URL = 'http://localhost:5135/api/v1/templater';

export class TemplaterApi {
  static async getTemplates(pageNumber: number = 1, pageSize: number = 10): Promise<PagedResponse<Template>> {
    const response = await fetch(`${API_BASE_URL}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    if (!response.ok) throw new Error('Failed to fetch templates');
    return response.json();
  }

  static async getTemplateById(id: string): Promise<Template> {
    const response = await fetch(`${API_BASE_URL}/${id}`);
    if (!response.ok) throw new Error('Failed to fetch template');
    return response.json();
  }

  static async createTemplate(template: CreateTemplateDto): Promise<Template> {
    const response = await fetch(API_BASE_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(template),
    });
    if (!response.ok) throw new Error('Failed to create template');
    return response.json();
  }

  static async updateTemplate(id: string, template: UpdateTemplateDto): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(template),
    });
    if (!response.ok) throw new Error('Failed to update template');
  }

  static async deleteTemplate(id: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Failed to delete template');
  }

  static async generatePdf(id: string, data: GeneratePdfDto): Promise<Blob> {
    const response = await fetch(`${API_BASE_URL}/${id}/generate`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    if (!response.ok) throw new Error('Failed to generate PDF');
    return response.blob();
  }
}
