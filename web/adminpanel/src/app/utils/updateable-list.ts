

export class UpdatableList<E> {
    private _elements: { [key: number]: E }
    private _keyGetter: (e: E) => number;

    constructor(keyGetter: (e: E) => number) {
        this._elements = {};
        this._keyGetter = keyGetter;
    }

    public clear() {
        this._elements = {};
    }

    public addRange(elements: Array<E>) {
        elements.forEach(element => {
            const elemKey = this._keyGetter(element);
            if (!(elemKey in this._elements)) {
                this._elements[elemKey] = element;
            } else {
                console.log(`[!] Error adding existing element ${element}`);
            }
        });
    }

    public deleteRange(elements: Array<E>) {
        elements.forEach(element => {
            const elemKey = this._keyGetter(element);
            if (elemKey in this._elements) {
                delete this._elements[elemKey];
            }
        });
    }

    public deleteSingle(elemKey: number) {
        if (elemKey in this._elements) {
            delete this._elements[elemKey];
        }
    }

    public getElements(): Array<E> {
        return Object.values(this._elements);
    }

    public getElementByKey(key: number): E | null {
        if (key in this._elements) {
            return this._elements[key];
        }
        return null;
    }
}