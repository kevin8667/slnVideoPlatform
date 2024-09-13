import { CanDeactivateFn } from '@angular/router';

export const unsavedChangesGuard: CanDeactivateFn<unknown> = (
  component: any,
  currentRoute,
  currentState,
  nextState
) => {
  if (component.articleForm && component.articleForm.touched) {
    // 提示用戶是否要離開頁面
    return confirm('你有未保存的更改。確定要離開嗎？');
  }
  return true;
};
