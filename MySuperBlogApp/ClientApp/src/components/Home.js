import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>It is super web application for blogging!!!</h1>
        <p>Created by Ryslan Shishmarev</p>
        <a href='https://github.com/RuslanShishmarev/MySuperBlogApp'>GutHub</a>
      </div>
    );
  }
}
