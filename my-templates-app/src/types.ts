export type Template = {
  id: string;
  name: string;
  htmlContent: string;
  createdAt: string;
  modifiedAt?: string;
};

export type CreateTemplateDto = {
  name: string;
  htmlContent: string;
};

export type UpdateTemplateDto = {
  name: string;
  htmlContent: string;
};

export type GeneratePdfDto = {
  data: Record<string, any>;
};

export type PagedResponse<T> = {
  item: T[];
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};
