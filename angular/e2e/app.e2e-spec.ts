import { AuditTestTemplatePage } from './app.po';

describe('AuditTest App', function() {
  let page: AuditTestTemplatePage;

  beforeEach(() => {
    page = new AuditTestTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
