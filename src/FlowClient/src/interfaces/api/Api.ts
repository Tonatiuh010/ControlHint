export interface Api {
  url: string;
  description: string;
  id: number;
}

export interface Param {
  name: string;
  contentType: string;
  value?: any;
  description: string;
  isRequired: boolean;
  id: number;
}

export interface Endpoint {
  route: string;
  requestType: string;
  api: Api;
  params: Param[];
  id: number;
}

export interface Step {
  sequence: number;
  isRequired: boolean;
  description: string;
  endpoint: Endpoint;
  id: number;
}

export interface Flow {
  name: string;
  steps?: Step[];
  status?: string;
  id: number;
}

