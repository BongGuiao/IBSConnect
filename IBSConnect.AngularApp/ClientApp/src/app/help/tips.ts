export class TipsDatabase {
  pointer: number;
  topic: string;

  private tips: string[] = [
    "<h4>Title</h4><hr/><p>This is a sample tip</p>"
  ];

  private topics: Record<string, string> = {
    /*AmEx Previous allocations*/
    "users/tips": "<h4>Title</h4><hr/><p>This is a sample topic</p>"
  }

  public setTopic(topic: string) {
    this.pointer = -1;
    this.topic = topic;
  }

  public get current() {
    return this.pointer === -1 ? this.topics[this.topic] : this.tips[this.pointer];
  }

  public next() {
    if (this.pointer == this.tips.length - 1) {
      this.pointer = 0;
      return;
    }
    this.pointer++;
  }

  public prev() {
    if (this.pointer <= 0) {
      this.pointer = this.tips.length - 1;
      return;
    }
    this.pointer--;
  }

  public random() {
    this.pointer = Math.round(Math.random() * this.tips.length - 1);
  }

}
