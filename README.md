
![脑图](https://raw.githubusercontent.com/acmol/balatro-feel/main/rouge-like-math.png)

## 声明
此项目基于[Balatro's Game Feel | Mix and Jam](https://github.com/mixandjam/balatro-feel) 为基础进行的开发。
可以看这个视频了解上边那个项目做了啥： [this video](https://youtu.be/I1dAZuWurw4).

# 当前进展


1. 完成了可以设置牌组，从牌组中抽牌功能
2. 完成了牌组与抽牌堆(Pile)的关联。抽牌时，会看到牌从抽牌堆往手牌中抽取；可以设置每种类型的卡牌每回合后补齐到的数据。
3. 完成了弃牌功能。
4. 完成了左、右键点击牌时的不同出牌效果（参考前边脑力）。
5. 部分完成了出牌按钮点击后，按钮隐藏、手牌下移、并将的出牌移到中央位置的动画。

TODO:

最简单可玩的demo要做的工作：
1. 制作图片0到99的四种花色的数字手牌，2到10可以参考扑克，剩下的确保左上右下角是数字，并且文字朝向正确，并加到游戏中可以被使用来创建手牌。
2. 将牌堆的显示数量与牌堆中剩余的数量挂购，并显示出来牌组中剩余数量。
3. 点击牌堆时，显示完整牌组信息，包括已经使用和剩余的牌分别有哪些。  
4. 当前进展中写的第5条，当前暂未显示出未选中牌所表达的计算优先级的概念，要想办法生成一些扣着的牌来表达这个。
5. 在Play Card点击后，使用一些数学公式插件来把出牌表达的公式和计算结果清晰的展示出来。
6. 出牌的公式点数计算逻辑和目标达成与否的判定
7. 目标达成后，重新抽目标牌的逻辑
8. 出牌手，目标没达成后回手的逻辑
9. 完善抽取目标牌的逻辑，不允许被抽出来的一张目标牌被改变顺序，但未被抽出的牌堆的几种
10. 制作几种不同的牌背面的图案，清晰的区分目标牌、数字手牌和操作手牌。
11. 公式非法时，出牌按钮处于特殊颜色提示用户当前出牌非法，但不影响用户按下；按下后提示非法，然后触发弃牌效果
   
## License

This project is licensed under the MIT License
