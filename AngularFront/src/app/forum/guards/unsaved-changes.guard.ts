import { CanDeactivateFn } from '@angular/router';
import { EditComponent } from '../edit/edit.component';

export const unsavedChangesGuard: CanDeactivateFn<unknown> = (
  component: any,
  currentRoute,
  currentState,
  nextState
) => {
  const editComponent = component as EditComponent;

  if (editComponent.isSubmitting) {
    return true; // 如果正在提交表單，允許離開頁面，不顯示確認提示
  }
  if (editComponent.articleForm && editComponent.articleForm.touched) {
    // 提示用戶是否要離開頁面
    return confirm('你有未保存的更改。確定要離開嗎？');
  }
  return true;
};
