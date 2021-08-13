export class GroupOfWaresTree {
    id: number;
    name: string;
    parent: GroupOfWaresTree;
    childs: Array<GroupOfWaresTree> = new Array();
    isExpanded: boolean;
    isLeaf: boolean;
    isChecked: boolean;
}
