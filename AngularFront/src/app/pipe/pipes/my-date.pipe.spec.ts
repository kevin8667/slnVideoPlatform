import { forumDatePipe } from './my-date.pipe';

describe('MyDatePipe', () => {
  it('create an instance', () => {
    const pipe = new forumDatePipe();
    expect(pipe).toBeTruthy();
  });
});
